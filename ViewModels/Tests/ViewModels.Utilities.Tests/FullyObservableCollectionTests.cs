using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DigitalElectronics.ViewModels.Utilities.Tests
{
    [TestFixture]
    public class FullyObservableCollectionTests : AssertionHelper
    {
        public class NotifyingTestClass : INotifyPropertyChanged
        {

            private int _id;

            public int Id
            {
                get => this._id;
                set
                {
                    if (this._id != value)
                    {
                        this._id = value;
                        this.RaisePropertyChanged(nameof(this.Id));
                    }
                }
            }


            private string _name;

            public string Name
            {
                get => this._name;
                set
                {
                    if (this._name != value)
                    {
                        this._name = value;
                        this.RaisePropertyChanged(nameof(this.Name));
                    }
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        FullyObservableCollection<NotifyingTestClass> TestCollection;
        NotifyingTestClass Fred;
        NotifyingTestClass Betty;
        List<NotifyCollectionChangedEventArgs> CollectionEventList;
        List<ItemPropertyChangedEventArgs> ItemEventList;

        [SetUp]
        public void Init()
        {
            Fred = new NotifyingTestClass() { Id = 1, Name = "Fred" };
            Betty = new NotifyingTestClass() { Id = 4, Name = "Betty" };

            TestCollection = new FullyObservableCollection<NotifyingTestClass>()
                {
                    Fred,
                    new NotifyingTestClass() {Id = 2, Name = "Barney" },
                    new NotifyingTestClass() {Id = 3, Name = "Wilma" }
                };

            CollectionEventList = new List<NotifyCollectionChangedEventArgs>();
            ItemEventList = new List<ItemPropertyChangedEventArgs>();
            TestCollection.CollectionChanged += (o, e) => CollectionEventList.Add(e);
            TestCollection.ItemPropertyChanged += (o, e) => ItemEventList.Add(e);
        }

        // Change existing member property: just ItemPropertyChanged(IPC) should fire
        [Test]
        public void DetectMemberPropertyChange()
        {
            TestCollection[0].Id = 7;

            Expect(CollectionEventList.Count, Is.EqualTo(0));

            Expect(ItemEventList.Count, Is.EqualTo(1), "IPC count");
            Expect(ItemEventList[0].PropertyName, Is.EqualTo(nameof(Fred.Id)), "Field Name");
            Expect(ItemEventList[0].CollectionIndex, Is.EqualTo(0), "Collection Index");
        }


        // Add new member, change property: CollectionPropertyChanged (CPC) and IPC should fire
        [Test]
        public void DetectNewMemberPropertyChange()
        {
            TestCollection.Add(Betty);

            Expect(TestCollection.Count, Is.EqualTo(4));
            Expect(TestCollection[3].Name, Is.EqualTo("Betty"));

            Expect(ItemEventList.Count, Is.EqualTo(0), "Item Event count");

            Expect(CollectionEventList.Count, Is.EqualTo(1), "Collection Event count");
            Expect(CollectionEventList[0].Action, Is.EqualTo(NotifyCollectionChangedAction.Add), "Action (add)");
            Expect(CollectionEventList[0].OldItems, Is.Null, "OldItems count");
            Expect(CollectionEventList[0].NewItems.Count, Is.EqualTo(1), "NewItems count");
            Expect(CollectionEventList[0].NewItems[0], Is.EqualTo(Betty), "NewItems[0] dereference");

            CollectionEventList.Clear();      // Empty for next operation
            ItemEventList.Clear();

            TestCollection[3].Id = 7;
            Expect(CollectionEventList.Count, Is.EqualTo(0), "Collection Event count");

            Expect(ItemEventList.Count, Is.EqualTo(1), "Item Event count");
            Expect(TestCollection[ItemEventList[0].CollectionIndex], Is.EqualTo(Betty), "Collection Index dereference");
        }


        // Remove member, change property: CPC should fire for removel, neither CPC nor IPC should fire for change
        [Test]
        public void CeaseListentingWhenMemberRemoved()
        {
            TestCollection.Remove(Fred);

            Expect(TestCollection.Count, Is.EqualTo(2));
            Expect(TestCollection.IndexOf(Fred), Is.Negative);

            Expect(ItemEventList.Count, Is.EqualTo(0), "Item Event count (pre change)");

            Expect(CollectionEventList.Count, Is.EqualTo(1), "Collection Event count (pre change)");
            Expect(CollectionEventList[0].Action, Is.EqualTo(NotifyCollectionChangedAction.Remove), "Action (remove)");
            Expect(CollectionEventList[0].OldItems.Count, Is.EqualTo(1), "OldItems count");
            Expect(CollectionEventList[0].NewItems, Is.Null, "NewItems count");
            Expect(CollectionEventList[0].OldItems[0], Is.EqualTo(Fred), "OldItems[0] dereference");

            CollectionEventList.Clear();      // Empty for next operation
            ItemEventList.Clear();

            Fred.Id = 7;
            Expect(CollectionEventList.Count, Is.EqualTo(0), "Collection Event count (post change)");
            Expect(ItemEventList.Count, Is.EqualTo(0), "Item Event count (post change)");
        }


        // Move member in list, change property: CPC should fire for move, IPC should fire for change
        [Test]
        public void MoveMember()
        {
            TestCollection.Move(0, 1);

            Expect(TestCollection.Count, Is.EqualTo(3));
            Expect(TestCollection.IndexOf(Fred), Is.GreaterThan(0));

            Expect(ItemEventList.Count, Is.EqualTo(0), "Item Event count (pre change)");

            Expect(CollectionEventList.Count, Is.EqualTo(1), "Collection Event count (pre change)");
            Expect(CollectionEventList[0].Action, Is.EqualTo(NotifyCollectionChangedAction.Move), "Action (move)");
            Expect(CollectionEventList[0].OldItems.Count, Is.EqualTo(1), "OldItems count");
            Expect(CollectionEventList[0].NewItems.Count, Is.EqualTo(1), "NewItems count");
            Expect(CollectionEventList[0].OldItems[0], Is.EqualTo(Fred), "OldItems[0] dereference");
            Expect(CollectionEventList[0].NewItems[0], Is.EqualTo(Fred), "NewItems[0] dereference");

            CollectionEventList.Clear();      // Empty for next operation
            ItemEventList.Clear();

            Fred.Id = 7;
            Expect(CollectionEventList.Count, Is.EqualTo(0), "Collection Event count (post change)");

            Expect(ItemEventList.Count, Is.EqualTo(1), "Item Event count (post change)");
            Expect(TestCollection[ItemEventList[0].CollectionIndex], Is.EqualTo(Fred), "Collection Index dereference");
        }


        // Clear list, chnage property: only CPC should fire for clear and neither for property change
        [Test]
        public void ClearList()
        {
            TestCollection.Clear();

            Expect(TestCollection.Count, Is.EqualTo(0));

            Expect(ItemEventList.Count, Is.EqualTo(0), "Item Event count (pre change)");

            Expect(CollectionEventList.Count, Is.EqualTo(1), "Collection Event count (pre change)");
            Expect(CollectionEventList[0].Action, Is.EqualTo(NotifyCollectionChangedAction.Reset), "Action (reset)");
            Expect(CollectionEventList[0].OldItems, Is.Null, "OldItems count");
            Expect(CollectionEventList[0].NewItems, Is.Null, "NewItems count");

            CollectionEventList.Clear();      // Empty for next operation
            ItemEventList.Clear();

            Fred.Id = 7;
            Expect(CollectionEventList.Count, Is.EqualTo(0), "Collection Event count (post change)");
            Expect(ItemEventList.Count, Is.EqualTo(0), "Item Event count (post change)");
        }
    }
}
