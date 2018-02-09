using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HelloTemplateSelector
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MyViewModel
            {
                MyItems = new List<IMyItem>
                {
                    new MyItemA { Name = "Andrey" },
                    new MyItemB { Content = "I'm ItemB!!!11" },
                    new MyItemA { Name = "loki2302" }
                }
            };
        }
    }

    public class MyViewModel
    {
        public IList<IMyItem> MyItems { get; set; }
    }

    public interface IMyItem
    {
    }

    public class MyItemA : IMyItem
    {
        public string Name { get; set; }
    }

    public class MyItemB : IMyItem
    {
        public string Content { get; set; }
    }

    public class MyItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TemplateForItemA { get; set; }
        public DataTemplate TemplateForItemB { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (!(item is IMyItem))
            {
                throw new ArgumentException(item.GetType().Name + " does not implement IMyItem", "item");
            }

            if (item is MyItemA)
            {
                return TemplateForItemA;
            }

            if (item is MyItemB)
            {
                return TemplateForItemB;
            }

            throw new ArgumentException("Unknown item type " + item.GetType().Name, "item");
        }
    }
}
