using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using EntityFrameworkExperiment2.DAL;
using EntityFrameworkExperiment2.DAL.Entities;
using EntityFrameworkExperiment2.DTO;

namespace EntityFrameworkExperiment2
{
    public class RenoteService
    {
        private readonly string _connectionStringName;

        public RenoteService(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public void Reset()
        {
            var connectionStringBuilder = new SqlCeConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString);
            var databaseFileName = connectionStringBuilder.DataSource;
            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
            }

            using (var context = new RenoteContext(_connectionStringName))
            {
                context.Database.Create();
            }
        }

        public AuthenticationDTO Authenticate(string userName)
        {
            return Run(context =>
                {
                    var user = context.Users.SingleOrDefault(u => u.UserName == userName);
                    if (user == null)
                    {
                        user = new User
                            {
                                UserName = userName
                            };
                        context.Users.Add(user);
                    }

                    var session = new Session
                        {
                            SessionToken = Convert.ToString(Guid.NewGuid()),
                            User = user
                        };
                    context.Sessions.Add(session);

                    context.SaveChanges();

                    return new AuthenticationDTO
                        {
                            SessionToken = session.SessionToken,
                            UserName = user.UserName
                        };
                });
        }

        public CompleteNoteDTO CreateNote(string sessionToken, string noteText, IList<string> tagNames)
        {
            return Run(context =>
                {
                    var user = GetUserBySessionTokenOrThrow(context, sessionToken);

                    var existingTags = context.Tags
                        .Where(t => t.UserId == user.UserId)
                        .Where(t => tagNames.Contains(t.TagName)).ToList();
                    var tagNamesToCreate = tagNames.Except(existingTags.Select(t => t.TagName));
                    var newTags = tagNamesToCreate.Select(tagNameToCreate => new Tag
                        {
                            TagName = tagNameToCreate,
                            User = user
                        }).ToList();
                    context.Tags.AddRange(newTags);

                    var allNoteTags = existingTags.Union(newTags).ToList();

                    var note = new Note
                        {
                            NoteText = noteText,
                            Tags = allNoteTags,
                            User = user
                        };
                    context.Notes.Add(note);

                    context.SaveChanges();

                    return MapNoteToCompleteNoteDTO(note);
                });
        }

        public CompleteNoteDTO GetNote(string sessionToken, int noteId)
        {
            return Run(context =>
                {
                    var user = GetUserBySessionTokenOrThrow(context, sessionToken);

                    var note = context.Notes
                        .Include(n => n.Tags)
                        .SingleOrDefault(n => n.NoteId == noteId);
                    if (note == null)
                    {
                        throw new Exception("no such note");
                    }

                    if (user.UserId != note.UserId)
                    {
                        throw new Exception("no access");
                    }

                    return MapNoteToCompleteNoteDTO(note);
                });
        }

        public CompleteNoteDTO UpdateNote(string sessionToken, int noteId, string noteText, IList<string> tagNames)
        {
            return Run(context =>
                {
                    var user = GetUserBySessionTokenOrThrow(context, sessionToken);

                    var note = context.Notes
                        .Include(n => n.Tags)
                        .SingleOrDefault(n => n.NoteId == noteId);
                    if (note == null)
                    {
                        throw new Exception("no such note");
                    }

                    if (user.UserId != note.UserId)
                    {
                        throw new Exception("no access");
                    }

                    var currentlyReferencedNoteTags = note.Tags;
                    
                    // unlink existing tags
                    var noteTagsToUnlink = currentlyReferencedNoteTags
                        .Where(t => !tagNames.Contains(t.TagName))
                        .ToList();
                    foreach (var noteTagToUnlink in noteTagsToUnlink)
                    {
                        note.Tags.Remove(noteTagToUnlink);
                    }

                    // delete unused tags
                    context.Tags.RemoveRange(noteTagsToUnlink.Where(t => t.Notes.Count() == 1));
                    
                    // create new tags
                    var tagsToCreateAndLink = tagNames
                        .Where(tagName => !currentlyReferencedNoteTags
                            .Select(t => t.TagName)
                            .Contains(tagName))
                            .Select(tagName => new Tag
                                {
                                    TagName = tagName,
                                    User = user
                                }).ToList();
                    context.Tags.AddRange(tagsToCreateAndLink);

                    // link new tags
                    foreach (var tagToCreateAndLink in tagsToCreateAndLink)
                    {
                        note.Tags.Add(tagToCreateAndLink);
                    }

                    note.NoteText = noteText;

                    context.SaveChanges();
                    
                    return MapNoteToCompleteNoteDTO(note);
                });
        }

        public void DeleteNote(string sessionToken, int noteId)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetTags(string sessionToken)
        {
            return Run(context =>
                {
                    var user = GetUserBySessionTokenOrThrow(context, sessionToken);
                    var tagNames = context.Tags
                        .Where(t => t.UserId == user.UserId)
                        .Select(t => t.TagName)
                        .ToList();

                    return tagNames;
                });
        }

        public IList<CompleteNoteDTO> FindNotes(string sessionToken, IList<string> tagNames)
        {
            return Run(context =>
                {
                    var user = GetUserBySessionTokenOrThrow(context, sessionToken);

                    var tagNamesCount = tagNames.Count;

                    var foundNotes = context.Notes
                        .Where(n => n.UserId == user.UserId)
                        .Where(n => n.Tags
                            .Select(t => t.TagName)
                            .Intersect(tagNames).Count() == tagNamesCount)
                        .Include(n => n.Tags)
                        .ToList();

                    return foundNotes.Select(MapNoteToCompleteNoteDTO).ToList();
                });
        }

        private static CompleteNoteDTO MapNoteToCompleteNoteDTO(Note note)
        {
            return new CompleteNoteDTO
                {
                    NoteId = note.NoteId,
                    NoteText = note.NoteText,
                    Tags = note.Tags.Select(t => t.TagName).ToList()
                };
        }

        private User GetUserBySessionTokenOrThrow(RenoteContext context, string sessionToken)
        {
            var session = context.Sessions.SingleOrDefault(s => s.SessionToken == sessionToken);
            if (session == null)
            {
                throw new Exception("no such session");
            }

            var user = context.Users.Single(u => u.UserId == session.UserId);
            return user;
        }

        private T Run<T>(Func<RenoteContext, T> func)
        {
            using (var context = new RenoteContext(_connectionStringName))
            //using (var transactionScope = new TransactionScope()) // EF throws when this is enabled
            {
                var wasError = false;
                try
                {
                    return func(context);
                }
                catch
                {
                    wasError = true;
                    throw;
                }
                finally
                {
                    if (!wasError)
                    {
                        //transactionScope.Complete();
                    }
                }
            }
        }
    }
}