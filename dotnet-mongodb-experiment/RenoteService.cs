using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using dotnet_mongodb_experiment.DTO;
using dotnet_mongodb_experiment.Entities;

namespace dotnet_mongodb_experiment
{
    public class RenoteService
    {
        private readonly MongoCollection<User> _userCollection;
        private readonly MongoCollection<Session> _sessionCollection;
        private readonly MongoCollection<Tag> _tagCollection;
        private readonly MongoCollection<Note> _noteCollection;

        public RenoteService(
            MongoCollection<User> userCollection, 
            MongoCollection<Session> sessionCollection, 
            MongoCollection<Tag> tagCollection, 
            MongoCollection<Note> noteCollection)
        {
            _userCollection = userCollection;
            _sessionCollection = sessionCollection;
            _tagCollection = tagCollection;
            _noteCollection = noteCollection;
        }

        public void RemoveEverything()
        {
            _userCollection.RemoveAll();
            _sessionCollection.RemoveAll();
            _tagCollection.RemoveAll();
            _noteCollection.RemoveAll();
        }

        public AuthenticationDTO Authenticate(string userName)
        {
            var user = _userCollection.FindOne(new QueryDocument("UserName", userName));
            if (user == null)
            {
                user = new User
                    {
                        UserName = userName
                    };

                _userCollection.Insert(user);
            }

            var session = new Session
                {
                    User = user
                };
            _sessionCollection.Insert(session);

            return new AuthenticationDTO
                {
                    SessionToken = Convert.ToString(session.Id),
                    UserName = userName
                };
        }

        private User GetUserBySessionTokenOrThrow(string sessionToken)
        {
            var session = _sessionCollection.FindOneById(new ObjectId(sessionToken));
            if (session == null)
            {
                throw new Exception("No such session");
            }

            var user = session.User;
            return user;
        }

        public CompleteNoteDTO CreateNote(string sessionToken, string noteText, IList<string> tagNames)
        {
            var user = GetUserBySessionTokenOrThrow(sessionToken);

            var tags = new List<Tag>();
            foreach (var tagName in tagNames)
            {
                var tag = _tagCollection.FindOne(Query<Tag>.EQ(t => t.TagName, tagName));
                if (tag == null)
                {
                    tag = new Tag
                        {
                            TagName = tagName, 
                            User = user
                        };
                    _tagCollection.Insert(tag);
                }

                tags.Add(tag);
            }

            var note = new Note
                {
                    Text = noteText,
                    Tags = tags,
                    User = user
                };
            _noteCollection.Insert(note);

            return MapNoteToCompleteNoteDTO(note);
        }

        private static CompleteNoteDTO MapNoteToCompleteNoteDTO(Note note)
        {
            return new CompleteNoteDTO
                {
                    NoteId = Convert.ToString(note.Id),
                    Text = note.Text,
                    User = new BriefUserDTO
                        {
                            UserId = Convert.ToString(note.User.Id),
                            UserName = note.User.UserName
                        },
                    Tags = note.Tags.Select(t => new BriefTagDTO
                        {
                            TagId = Convert.ToString((object) t.Id),
                            TagName = t.TagName
                        }).ToList()
                };
        }

        public CompleteNoteDTO UpdateNote(string sessionToken)
        {
            var user = GetUserBySessionTokenOrThrow(sessionToken);
            throw new NotImplementedException();
        }

        public CompleteNoteDTO GetNote(string sessionToken, string noteId)
        {
            var user = GetUserBySessionTokenOrThrow(sessionToken);
            var note = _noteCollection.FindOneById(new ObjectId(noteId));
            if (note == null)
            {
                throw new Exception("no such note");
            }

            if (!note.User.Id.Equals(user.Id))
            {
                throw new Exception("access denied");
            }

            return MapNoteToCompleteNoteDTO(note);
        }

        public IList<CompleteNoteDTO> GetAllNotes(string sessionToken)
        {
            var user = GetUserBySessionTokenOrThrow(sessionToken);
            var notes = _noteCollection.Find(Query<Note>.EQ(n => n.User.Id, user.Id));
            return notes.Select(MapNoteToCompleteNoteDTO).ToList();
        }

        public void DeleteNote(string sessionToken)
        {
            var user = GetUserBySessionTokenOrThrow(sessionToken);
            throw new NotImplementedException();
        }

        public void GetTags(string sessionToken)
        {
            var user = GetUserBySessionTokenOrThrow(sessionToken);
            throw new NotImplementedException();
        }

        public void FindNotes(string sessionToken, IList<string> tagNames)
        {
            var user = GetUserBySessionTokenOrThrow(sessionToken);
            throw new NotImplementedException();
        }
    }
}