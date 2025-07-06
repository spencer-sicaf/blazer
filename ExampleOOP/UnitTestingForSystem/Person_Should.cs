using FluentAssertions;
using OOPLibrary;

namespace UnitTestingForSystem
{
    public class Person_Should
    {
        #region Valid Data Tests
        //Fact: one test, test body (what makes up the test) contains all the setup,
        //execution and evaluation of the excution in a single test, Fact Tests run only ONCE
        //Theory: allow for multiple executions of the same test.
        //Takes in different inputs 
        // To specify our input the Test must have parameters and the data supplied to the parameters through 
        //[InlineData(...)]

        //Test our Constructor(s)

        //If a default exists, test that for sure and check that the default values are set properly.
        [Fact]
        public void Create_Instance_Using_Default_Constructor()
        {
            //Exceptions
            string expectedFirstName = "Unknown";
            string expectedLastName = "Unknown";

            //Action
            //sut - Subject Under Test
            Person sut = new(); //new() = new Person()

            //Assertions (evaluation of the result of the action)
            sut.FirstName.Should().Be(expectedFirstName);
            sut.LastName.Should().Be(expectedLastName);
            sut.ResidentAddress.Should().BeNull();
            sut.Positions.Count.Should().Be(0);
            sut.FullName.Should().Be($"{expectedFirstName} {expectedLastName}");
        }

        //Greedy or Overloaded Constructors (in this class - greedy constructors)

        [Fact]
        public void Create_Instance_Using_Greedy_Constructor_With_Positions()
        {
            //Expectations and Arrangement
            string expectedFirstName = "Bob";
            string expectedLastName = "Frank";
            ResidentAddress expectedResidentAddress = new ResidentAddress(12, "Maple St.", "Edmonton", "AB", "T5T 5T5");
            List<Employment> positions =
            [
                new Employment("Random", SupervisoryLevel.TeamLeader, DateTime.Today),
                new Employment("Title2", SupervisoryLevel.DepartmentHead, DateTime.Parse("2016/07/28"))
            ];

            //Actions
            Person sut = new(expectedFirstName, expectedLastName, positions, expectedResidentAddress);

            //Assertions
            sut.FirstName.Should().Be(expectedFirstName);
            sut.LastName.Should().Be(expectedLastName);
            sut.ResidentAddress.Should().Be(expectedResidentAddress);
            //Check that the count of the collection matches the count of what we gave it
            sut.Positions.Should().HaveCount(positions.Count);
            sut.FullName.Should().Be($"{expectedFirstName} {expectedLastName}");
        }

        //Testing providing a null to the Collection
            //Testing that we don't get a null reference error and we handled it in the constructor.
        [Fact] public void Create_Instance_With_Greedy_Constructor_Without_Position()
        {
            //Expectations and Arrangement
            string expectedFirstName = "Bob";
            string expectedLastName = "Frank";
            ResidentAddress expectedResidentAddress = new ResidentAddress(12, "Maple St.", "Edmonton", "AB", "T5T 5T5");

            //Action
            //Providing a null for the Collection (List of Employment)
            Person sut = new(expectedFirstName, expectedLastName, null, expectedResidentAddress);

            //Assertion
            sut.FirstName.Should().Be(expectedFirstName);
            sut.LastName.Should().Be(expectedLastName);
            sut.ResidentAddress.Should().Be(expectedResidentAddress);
            //Making sure that there isn't a null set for the List - if the null is not handled in the constructor this will error and give a null reference error.
            sut.Positions.Count.Should().Be(0);
            sut.FullName.Should().Be($"{expectedFirstName} {expectedLastName}");
        }

        //Test our Methods
        [Fact]
        public void Create_CSV_String()
        {
            //Expectations and Arrangement
            string expectedFirstName = "Bob";
            string expectedLastName = "Frank";
            ResidentAddress expectedResidentAddress = new ResidentAddress(12, "Maple St.", "Edmonton", "AB", "T5T 5T5");

            //Action
            //Providing a null for the Collection (List of Employment)
            Person sut = new(expectedFirstName, expectedLastName, null, expectedResidentAddress);

            sut.ToString().Should().Be($"{expectedFirstName},{expectedLastName},{expectedResidentAddress.Number},{expectedResidentAddress.Street},{expectedResidentAddress.City},{expectedResidentAddress.Region},{expectedResidentAddress.PostalCode}");
        }

        [Fact]
        public void Change_FirstName()
        {
            //Arrange
            Person sut = MakePersonWithoutPositions();
            string expectedFirstName = "Tina";

            //Action
            sut.FirstName = expectedFirstName;

            //Assert
            sut.FirstName.Should().Be(expectedFirstName);
            sut.FullName.Should().Be($"{expectedFirstName} {sut.LastName}");
        }

        [Fact]
        public void Add_Employment_No_Previous_Employment()
        {
            //Arrange
            Person sut = MakePersonWithoutPositions();
            Employment newEmployment = new("King Bob", SupervisoryLevel.Entry, DateTime.Today);

            //Action
            sut.AddEmployment(newEmployment);

            //Assert
            sut.Positions.Count.Should().Be(1);
            //sut.Positions.Count.Should().Equals(1);
        }
        #endregion

        #region Invalid Data Test
        //Test the Constructor and provide bad values

        //Testing a string will always be a theory test and always provided a null, an empty string, and whitespace.
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public void Create_New_Greedy_Instance_Throws_FirstName_Exception(string firstName)
        {
            //Expectations and Arrangement
            //string expectedFirstName = "Bob";
            string lastName = "Frank";
            ResidentAddress residentAddress = new ResidentAddress(12, "Maple St.", "Edmonton", "AB", "T5T 5T5");

            //Action
            Action action = () => new Person(firstName, lastName, null, residentAddress);

            //Assurtion
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public void Create_New_Greedy_Instance_Throws_LastName_Exception(string lastName)
        {
            //Expectations and Arrangement
            string firstName = "Bob";
            //string lastName = "Frank";
            ResidentAddress residentAddress = new ResidentAddress(12, "Maple St.", "Edmonton", "AB", "T5T 5T5");

            //Action
            Action action = () => new Person(firstName, lastName, null, residentAddress);

            //Assurtion
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddEmployment_When_Position_Level_Same()
        {
            //Arrange
            Person sut = MakePersonWithPositions();
            Employment position = new Employment("Random 2", SupervisoryLevel.TeamLeader, DateTime.Today);

            //Action
            Action action = () => sut.AddEmployment(position);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage("*already has that level*");
        }
        #endregion

        #region Utilities
        private Person MakePersonWithoutPositions()
        {
            string firstName = "Bob";
            string lastName = "Frank";
            ResidentAddress residentAddress = new ResidentAddress(12, "Maple St.", "Edmonton", "AB", "T5T 5T5");

            return new Person(firstName, lastName, null, residentAddress);
        }

        private Person MakePersonWithPositions()
        {
            string firstName = "Bob";
            string lastName = "Frank";
            ResidentAddress residentAddress = new ResidentAddress(12, "Maple St.", "Edmonton", "AB", "T5T 5T5");

            List<Employment> positions =
            [
                new Employment("Random", SupervisoryLevel.TeamLeader, DateTime.Today),
                new Employment("Title2", SupervisoryLevel.DepartmentHead, DateTime.Parse("2016/07/28"))
            ];


            return new Person(firstName, lastName, positions, residentAddress);
        }
        #endregion
    }
}
