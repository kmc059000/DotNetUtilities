using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuerySorter.Tests
{
    [TestClass]
    public class QuerySorterTests
    {
        private static readonly List<Person> People = new List<Person>()
        {
            new Person("AFirstName", "ALastName", 45),
            new Person("BFirstName", "ALastName", 18),

            new Person("AFirstName", "BLastName", 20),
            new Person("BFirstName", "BLastName", 36),

            new Person("AFirstName", "CLastName", 6),
            new Person("BFirstName", "DLastName", 76),

        };

        [TestMethod]
        public void QuerySorter_CanSortQuerayableAscending()
        {
            var sorter = new PersonQuerySorter();

            var queryable = People.AsQueryable();

            var sorted = sorter.Sort(queryable, PersonQuerySorter.SortName.FirstName, ListSortDirection.Ascending);
            Assert.IsTrue(People.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).SequenceEqual(sorted));

            sorted = sorter.Sort(queryable, PersonQuerySorter.SortName.LastName, ListSortDirection.Ascending);
            Assert.IsTrue(People.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).SequenceEqual(sorted));

            sorted = sorter.Sort(queryable, PersonQuerySorter.SortName.Age, ListSortDirection.Ascending);
            Assert.IsTrue(People.OrderBy(x => x.Age).SequenceEqual(sorted));

            sorted = sorter.Sort(queryable, PersonQuerySorter.SortName.DOB, ListSortDirection.Ascending);
            Assert.IsTrue(People.OrderBy(x => x.DOB).SequenceEqual(sorted));
        }

        [TestMethod]
        public void QuerySorter_CanSortQuerayableDescending()
        {
            var sorter = new PersonQuerySorter();

            var queryable = People.AsQueryable();

            var sorted = sorter.Sort(queryable, PersonQuerySorter.SortName.FirstName, ListSortDirection.Descending);
            Assert.IsTrue(People.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName).SequenceEqual(sorted));

            sorted = sorter.Sort(queryable, PersonQuerySorter.SortName.LastName, ListSortDirection.Descending);
            Assert.IsTrue(People.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName).SequenceEqual(sorted));

            sorted = sorter.Sort(queryable, PersonQuerySorter.SortName.Age, ListSortDirection.Descending);
            Assert.IsTrue(People.OrderByDescending(x => x.Age).SequenceEqual(sorted));

            sorted = sorter.Sort(queryable, PersonQuerySorter.SortName.DOB, ListSortDirection.Descending);
            Assert.IsTrue(People.OrderByDescending(x => x.DOB).SequenceEqual(sorted));
        }

        [TestMethod]
        public void QuerySorter_CanSortQuerayableWithInverted()
        {
            var sorter = new PersonQuerySorter();

            var queryable = People.AsQueryable();

            var sorted = sorter.Sort(queryable, PersonQuerySorter.SortName.FirstNameAndOppositeLastName, ListSortDirection.Ascending);
            Assert.IsTrue(People.OrderBy(x => x.FirstName).ThenByDescending(x => x.LastName).SequenceEqual(sorted));
        }

        [TestMethod]
        public void QuerySorter_CanSortEnumerableAscending()
        {
            var sorter = new PersonQuerySorter();

            var sorted = sorter.Sort(People, PersonQuerySorter.SortName.FirstName, ListSortDirection.Ascending);
            Assert.IsTrue(People.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).SequenceEqual(sorted));

            sorted = sorter.Sort(People, PersonQuerySorter.SortName.LastName, ListSortDirection.Ascending);
            Assert.IsTrue(People.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).SequenceEqual(sorted));

            sorted = sorter.Sort(People, PersonQuerySorter.SortName.Age, ListSortDirection.Ascending);
            Assert.IsTrue(People.OrderBy(x => x.Age).SequenceEqual(sorted));

            sorted = sorter.Sort(People, PersonQuerySorter.SortName.DOB, ListSortDirection.Ascending);
            Assert.IsTrue(People.OrderBy(x => x.DOB).SequenceEqual(sorted));
        }

        [TestMethod]
        public void QuerySorter_CanSortEnumerableDescending()
        {
            var sorter = new PersonQuerySorter();

            var sorted = sorter.Sort(People, PersonQuerySorter.SortName.FirstName, ListSortDirection.Descending);
            Assert.IsTrue(People.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName).SequenceEqual(sorted));

            sorted = sorter.Sort(People, PersonQuerySorter.SortName.LastName, ListSortDirection.Descending);
            Assert.IsTrue(People.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName).SequenceEqual(sorted));

            sorted = sorter.Sort(People, PersonQuerySorter.SortName.Age, ListSortDirection.Descending);
            Assert.IsTrue(People.OrderByDescending(x => x.Age).SequenceEqual(sorted));

            sorted = sorter.Sort(People, PersonQuerySorter.SortName.DOB, ListSortDirection.Descending);
            Assert.IsTrue(People.OrderByDescending(x => x.DOB).SequenceEqual(sorted));
        }

        [TestMethod]
        public void QuerySorter_CanSortEnumerableWithInverted()
        {
            var sorter = new PersonQuerySorter();

            var sorted = sorter.Sort(People, PersonQuerySorter.SortName.FirstNameAndOppositeLastName, ListSortDirection.Ascending);
            Assert.IsTrue(People.OrderBy(x => x.FirstName).ThenByDescending(x => x.LastName).SequenceEqual(sorted));
        }

        private class PersonQuerySorter : QuerySorter<Person, PersonQuerySorter.SortName>
        {
            public enum SortName
            {
                FirstName,
                LastName,
                Age,
                DOB,
                FirstNameAndOppositeLastName
            }

            protected override IEnumerable<SortExpression<Person>> GetSortExpressions(SortName sortname, ListSortDirection direction)
            {
                switch (sortname)
                {
                    case SortName.FirstName:
                        yield return SortBy(x => x.FirstName, direction);
                        yield return SortBy(x => x.LastName, direction);
                        break;
                    case SortName.LastName:
                        yield return SortBy(x => x.LastName, direction);
                        yield return SortBy(x => x.FirstName, direction);
                        break;
                    case SortName.Age:
                        yield return SortBy(x => x.Age, direction);
                        break;
                    case SortName.DOB:
                        yield return SortBy(x => x.DOB, direction);
                        break;
                    case SortName.FirstNameAndOppositeLastName:
                        yield return SortBy(x => x.FirstName, direction);
                        yield return SortBy(x => x.LastName, OppositeDirection(direction));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(sortname), sortname, null);
                }
            }
        }

        private class Person
        {
            public Person(string firstName, string lastName, int age)
            {
                FirstName = firstName;
                LastName = lastName;
                Age = age;
                DOB = DateTime.Now.AddYears(-age);
            }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public DateTime DOB { get; set; }
        }
    }
}
