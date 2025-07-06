using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging.Console;
using Microsoft.JSInterop;
using OOPLibrary;
using System;

namespace OOPWebApp.Components.Pages.Examples
{

    public partial class DataEntry
    {
        private string feedback = string.Empty;
        private List<string> errorMsgs = [];
        private List<string> employees = [];

        //Creating variables to store the form information
        //Match what is in the class for datatypes
        private string empTitle = string.Empty;
        private DateTime empStartDate = DateTime.Today;
        private SupervisoryLevel empLevel;
        private double empYears = 0.0;
        private Employment employment = null;
        private string employee = string.Empty;

        //injected service into your application
        //injected services they need to be coded as properties, typically auto-implemented.
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        //Inject a service to navigate between pages
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            employees = new List<string>() 
            {
                "Tina Caron",
                "Mike Stuparyk",
                "Duerr Garcia",
                "Fred Bob",
            };
        }
        private void OnCollect()
        {
            feedback = string.Empty; //remove any old messages
            errorMsgs.Clear(); //remove any old error message

            //primitive validation
                // presence
                // datatype/pattern
                // range of value
            //Business Rules
                // Title must present, must have at least character
                // start date cannot be in the future
                // years cannot be less than zero

            if(string.IsNullOrWhiteSpace(empTitle) )
            {
                errorMsgs.Add("Title is required.");
            }
            if(empStartDate > DateTime.Today)
            {
                errorMsgs.Add("Start Date cannot be in the future.");
            }
            if (empYears < 0.0)
            {
                errorMsgs.Add("Years must be 0 or greater.");
            }
            if (errorMsgs.Count == 0)
            {
                //at this point the data is acceptable based off the form validation

                //we can now process the data

                //if you are using a class to collect and hold your data
                //  You need to be concerned with how the class coding
                //  handled any errors
                // this normally means some type of try/catch processing
                try
                {
                    //create an instance of the Employment class
                    //  we will write this to a data file
                    //remember that if an error occurs while creating your instance
                    //the instance will throw an exception!

                    employment = new Employment(empTitle, empLevel, empStartDate, empYears);

                    //write the class data as a csv string to a csv text file
                    //required:
                    //  a) know the file location
                    //  b) have a technique to write to a file
                    //      1) SteamReading/StreamWriter
                    //      2) use System.IO.File methods (this is simpler and we will use this)
                    string fileName = @"Data/Employments.csv";
                    string line = $"{employment}\n";
                    File.AppendAllText(fileName, line);
                    feedback = $"Entered Data: {empTitle}, {empStartDate.ToString("MMM dd yyyy")}, {empLevel}, {empYears} \n\nData saved to file.";
                }
                catch (ArgumentNullException ex)
                {
                    errorMsgs.Add($"Missing Data: {GetInnerException(ex).Message}");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    errorMsgs.Add($"Data Range: {GetInnerException(ex).Message}");
                }
                catch (ArgumentException ex)
                {
                    errorMsgs.Add($"Data Value: {GetInnerException(ex).Message}");
                }
                catch (FormatException ex)
                {
                    errorMsgs.Add($"Data Format: {GetInnerException(ex).Message}");
                }
                catch (Exception ex)
                {
                    errorMsgs.Add($"System error: {GetInnerException(ex).Message}");
                }
            }
        }
        private void GoToReport()
        {
            NavigationManager.NavigateTo("employmentreport");
        }
        private Exception GetInnerException(Exception ex)
        {
            //drill down into your Exception until there are no more inner exceptions
            //at this point you have the "real" error
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }
        private async void ClearForm()
        {
            //issue as SimpleConsoleFormatterOptions prompt dialogue to the user to confirm the form clearing.
            object[] messageLine = new object[] { "Clearing with lose all unsaved data. Are you sure you want to clear the form?" };
            if(await JSRuntime.InvokeAsync<bool>("confirm", messageLine))
            {
                feedback = string.Empty;
                errorMsgs.Clear();
                empTitle = string.Empty;
                empLevel = SupervisoryLevel.Entry;
                empYears = 0;
                empStartDate = DateTime.Today;
                //If you are in an async function use
                await InvokeAsync(StateHasChanged);
                //If not in an async function use 
                //StateHasChanged();
            }
        }
    }
}
