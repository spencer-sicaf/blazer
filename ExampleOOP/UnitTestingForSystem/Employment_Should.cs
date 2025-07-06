using FluentAssertions;
using OOPLibrary;

namespace UnitTestingForSystem
{
    public class Employment_Should
    {

        #region Valid Data
        //the type [Fact] says to run the test once
        [Fact]
        public void Create_New_Default_Instance()
        {
            //Where - Arrange setup
            string expectedTitle = "unknown";
            SupervisoryLevel expectedLevel = SupervisoryLevel.TeamMember;
            DateTime expectedStartDate = DateTime.Today;
            double expectedYears = 0;
            double expectedEmploymentYears = 0;

            //When - Act execution
            Employment actual = new Employment();

            //Then - Assert check
            actual.Title.Should().Be(expectedTitle);
            actual.Level.Should().Be(expectedLevel);
            actual.StartDate.Should().Be(expectedStartDate);
            actual.Years.Should().Be(expectedYears);
            actual.EmploymentYears.Should().Be(expectedEmploymentYears);
        }

        [Fact]
        public void Create_New_Greedy_Instance()
        {
            //Where - Arrange setup
            string expectedTitle = "SAS Lead";
            SupervisoryLevel expectedLevel = SupervisoryLevel.TeamLeader;
            DateTime expectedStartDate = new DateTime(2020, 10, 24);
            double expectedYears = 3.6;
            TimeSpan days = DateTime.Today - expectedStartDate;
            double expectedEmploymentYears = Math.Round((days.Days / 365.25), 2);

            //When - Act execution
            Employment actual = new Employment(expectedTitle, expectedLevel, expectedStartDate, expectedYears);

            //Then - Assert check
            actual.Title.Should().Be(expectedTitle);
            actual.Level.Should().Be(expectedLevel);
            actual.StartDate.Should().Be(expectedStartDate);
            actual.Years.Should().Be(expectedYears);
            actual.EmploymentYears.Should().Be(expectedEmploymentYears);
        }

        //Depending on whether you adjusted the years when the default for years parameter
        //  is used the greedy constructor, this test may or may not work (see note in greedy constructor)
        [Fact]
        public void Create_New_Greedy_Instance_With_Years_Default()
        {
            //Where - Arrange setup
            string expectedTitle = "SAS Lead";
            SupervisoryLevel expectedLevel = SupervisoryLevel.TeamLeader;
            DateTime expectedStartDate = new DateTime(2020, 10, 24);

            //if code not in greedy constructor, expectedYears will be 0.0

            TimeSpan days = DateTime.Today - expectedStartDate;
            double expectedYears = Math.Round((days.Days / 365.25), 1);
            double expectedEmploymentYears = Math.Round((days.Days / 365.25), 2);

            //When - Act execution
            Employment actual = new Employment(expectedTitle, expectedLevel, expectedStartDate);

            //Then - Assert check
            actual.Title.Should().Be(expectedTitle);
            actual.Level.Should().Be(expectedLevel);
            actual.StartDate.Should().Be(expectedStartDate);
            actual.Years.Should().Be(expectedYears);
            actual.EmploymentYears.Should().Be(expectedEmploymentYears);
        }

        [Fact]
        public void Create_New_Greedy_Instance_With_Year_Zero()
        {
            //Where - Arrange setup
            string title = "SAS Lead";
            SupervisoryLevel level = SupervisoryLevel.TeamLeader;
            DateTime startDate = DateTime.Today;
            double years = 0.0;
            TimeSpan days = DateTime.Today - startDate;
            double expectedEmploymentYears = Math.Round((days.Days / 365.25), 2);

            //Checks
            double expectedYears = 0.0;

            //When - Execution Action
            Employment action = new Employment(title, level, startDate, years);

            //Then - Assertions checks
            action.Years.Should().Be(expectedYears);
            action.EmploymentYears.Should().Be(expectedEmploymentYears);
        }

        [Fact]
        public void Change_the_Title()
        {
            //Where - Arrange setup
            string Title = "SAS Lead";
            SupervisoryLevel Level = SupervisoryLevel.TeamLeader;
            DateTime StartDate = new DateTime(2020, 10, 24);
            TimeSpan days = DateTime.Today - StartDate;
            double Years = Math.Round((days.Days / 365.25), 1);
            Employment sut = new Employment(Title, Level, StartDate, Years);

            string expectedTitle = "Development Head";

            //When - Act execution
            //sut -> subject under test
            sut.Title = "Development Head";

            //Then - Assert check
            sut.Title.Should().Be(expectedTitle);
        }

        [Fact]
        public void Set_The_SupervisoryLevel()
        {
            //Where - Arrange setup
            string Title = "SAS Lead";
            SupervisoryLevel Level = SupervisoryLevel.TeamLeader;
            DateTime StartDate = new DateTime(2020, 10, 24);
            TimeSpan days = DateTime.Today - StartDate;
            double Years = Math.Round((days.Days / 365.25), 1);
            Employment sut = new Employment(Title, Level, StartDate, Years);
            SupervisoryLevel expectedLevel = SupervisoryLevel.Supervisor;

            //When - Act execution
            sut.SetEmploymentResponsibilityLevel(SupervisoryLevel.Supervisor);

            //Then - Assert check
            sut.Level.Should().Be(expectedLevel);

        }

        [Fact]
        public void Set_The_Correct_StartDate()
        {
            //Where - Arrange setup
            string Title = "SAS Lead";
            SupervisoryLevel Level = SupervisoryLevel.TeamLeader;
            DateTime StartDate = new DateTime(2020, 10, 24);
            TimeSpan days = DateTime.Today - StartDate;
            double Years = Math.Round((days.Days / 365.25), 1);
            Employment sut = new Employment(Title, Level, StartDate, Years);
            DateTime expectedDate = new DateTime(2019, 10, 24);

            //add the generation of the years when the date is updated
            //this assumes that this is the most current employment

            days = DateTime.Today - expectedDate;
            double expectedyears = Math.Round((days.Days / 365.25), 1);

            //When - Act execution
            sut.CorrectStartDate(new DateTime(2019, 10, 24));

            //Then - Assert check
            sut.StartDate.Should().Be(expectedDate);
            sut.Years.Should().Be(expectedyears);
        }


        [Fact]
        public void Create_CSV_String()
        {
            string Title = "SAS Lead";
            SupervisoryLevel Level = SupervisoryLevel.TeamLeader;
            DateTime StartDate = new DateTime(2020, 10, 24);
            TimeSpan days = DateTime.Today - StartDate;
            double Years = Math.Round((days.Days / 365.25), 1);
            Employment sut = new Employment(Title, Level, StartDate, Years);
            string expectedCSV = $"SAS Lead,TeamLeader,Oct. 24 2020,{Years}";

            //When - Act execution
            string actual = sut.ToString();

            //Then - Assert check
            actual.Should().Be(expectedCSV);

        }


        #endregion

        #region Invalid Data
        // the type [Theory] requires the test to be run once for each [InlineData]
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public void Create_New_Greedy_Instance_Throws_Title_Exception(string title)
        {
            //Where - Arrange setup
            //string Title = "SAS Lead";
            SupervisoryLevel Level = (SupervisoryLevel)1;
                //Typecast is wrapping the Datatype in brackets () before the value you want to change to that datatype.
                //Some types cannot be automatically cast to other type, for example string to int 
                // Bad Cast (string to int): int test = (int)"test";
                // Valid Cast (double to int): int test = (int)2.0;
            DateTime StartDate = DateTime.Today;
            double Years = 0;

            //When - Act execution
            Action action = () => new Employment(title, Level, StartDate, Years);

            //Then - Assert check
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Create_New_Greedy_Instance_Throws_SupervisorLevel_Exception()
        {
            //Where - Arrange setup
            string title = "SAS Lead";
            //create an invalid enum value 
            SupervisoryLevel level = (SupervisoryLevel)15;
            DateTime startdate = DateTime.Today;
            double years = 0;

            //When - Act execution
            Action action = () => new Employment(title, level, startdate, years);

            //Then - Assert check
            //Using * to represent any value (like % in a SQL LIKE check)
            action.Should().Throw<ArgumentException>().WithMessage("*15*");
            //FIELD LIKE '%Test%'
        }

        [Fact]
        public void Create_New_Greedy_Instance_Throws_StartDate_Future_Exception()
        {
            //Where - Arrange setup
            string Title = "SAS Lead";
            SupervisoryLevel Level = SupervisoryLevel.TeamMember;
            DateTime StartDate = DateTime.Parse("2902/10/24");
            double Years = 0;

            //When - Act execution
            Action action = () => new Employment(Title, Level, StartDate, Years);

            //Then - Assert check
            action.Should().Throw<ArgumentException>().WithMessage("*future*");
        }

        [Fact]
        public void Create_New_Greedy_Instance_Throws_Negative_Years_Exception()
        {
            //Where - Arrange setup
            string Title = "SAS Lead";
            SupervisoryLevel Level = SupervisoryLevel.TeamMember;
            DateTime StartDate = DateTime.Today;
            double Years = -5.5;

            //When - Act execution
            Action action = () => new Employment(Title, Level, StartDate, Years);

            //Then - Assert check
            action.Should().Throw<ArgumentException>().WithMessage($"*{Years}*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("      ")]
        public void Directly_Change_Title_Throws_Exception(string title)
        {
            //Where - Arrange setup
            string Title = "SAS Lead";
            SupervisoryLevel Level = SupervisoryLevel.TeamMember;
            DateTime StartDate = DateTime.Today;
            double Years = 0;
            Employment sut = new Employment(Title, Level, StartDate, Years);

            //When - Act execution
            //Action performs the test we want on the code after the () =>
                //Whatever code we want to execute should be put into an action in order to ensure that the correct exception is thrown.
            Action action = () => sut.Title = title;

            //Then - Assert check
            action.Should().Throw<ArgumentNullException>();
        }

        //DO NOT use if your class demonstration has made Years set private
        [Fact]
        public void Directly_Change_Years_Throws_Exception()
        {
            //Where - Arrange setup
            string Title = "SAS Lead";
            SupervisoryLevel Level = SupervisoryLevel.TeamMember;
            DateTime StartDate = DateTime.Today;
            double Years = 0;
            Employment sut = new Employment(Title, Level, StartDate, Years);

            //When - Act execution
            Action action = () => sut.Years = -5.5;

            //Then - Assert check
            action.Should().Throw<ArgumentException>().WithMessage("*-5.5*");
        }

        [Fact]
        public void Set_The_SupervisoryLevel_Throws_Exception()
        {
            //Where - Arrange setup
            string Title = "SAS Lead";
            SupervisoryLevel Level = SupervisoryLevel.TeamLeader;
            DateTime StartDate = new DateTime(2020, 10, 24);
            TimeSpan days = DateTime.Today - StartDate;
            double Years = Math.Round((days.Days / 365.25), 1);
            Employment sut = new Employment(Title, Level, StartDate, Years);
            SupervisoryLevel badLevel = (SupervisoryLevel)15;
            //When - Act execution
            Action action = () => sut.SetEmploymentResponsibilityLevel(badLevel);

            //Then - Assert check
            action.Should().Throw<ArgumentException>().WithMessage("*15*");

        }

        [Fact]
        public void Set_The_Correct_StartDate_Throws_Exception()
        {
            //Where - Arrangement setup
            string Title = "SAS Lead";
            SupervisoryLevel Level = SupervisoryLevel.TeamLeader;
            DateTime StartDate = new DateTime(2020, 10, 24);
            Employment sut = new Employment(Title, Level, StartDate);

            //When - Act execution
            Action action = () => sut.CorrectStartDate(new DateTime(2919, 10, 24));

            //Then - Assert check
            action.Should().Throw<ArgumentException>().WithMessage("*future*");

        }
        #endregion
    }
}