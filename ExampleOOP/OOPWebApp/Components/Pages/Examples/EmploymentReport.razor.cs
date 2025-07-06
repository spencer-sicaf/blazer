using OOPLibrary;

namespace OOPWebApp.Components.Pages.Examples
{
    public partial class EmploymentReport
    {
        private string feedback = string.Empty;
        private List<string> errorMsgs = [];

        private List<Employment> employments = [];

        protected override void OnInitialized()
        {
            ReadData();
            base.OnInitialized();
        }

        private void ReadData()
        {
            feedback = string.Empty;
            errorMsgs.Clear();

            //Use our File IO to Read the data in from our file.

            string fileName = @"Data/Employments.csv";
            int lineCounter = 0;
            string[] employmentData = File.ReadAllLines(fileName);
            foreach(var line in employmentData)
            {
                lineCounter++;
                //if(Employment.TryParse(line, out Employment result))
                //{
                //    employments.Add(result);
                //}
                //else
                //{
                //    errorMsgs.Add($"Record Error: {lineCounter}: Unable to read record");
                //}

                //Directly using Parse
                try
                {
                    employments.Add(Employment.Parse(line));
                }
                catch(Exception ex)
                {
                    errorMsgs.Add($"Record Error: {lineCounter}: {GetInnerException(ex).Message}");
                }
            }
        }

        private Exception GetInnerException(Exception ex)
        {
            //drill down into your Exception until there are no more inner exceptions
            //at this point you have the "real" error
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }
    }
}
