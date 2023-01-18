

/*
 * Author : Krushna Jena
 * Created Date : 1/19/2023
 * Description : Console App created to get waether forcast details
 * Note : file path of the file may create issue during read the file
 */

Console.WriteLine("Enter City Name");
string? cityName = Console.ReadLine();

if (!String.IsNullOrEmpty(cityName))
{
    try
    {
        string fileName = "in.csv";
        dynamic currentDirectory="";
        if (Environment.CurrentDirectory.Contains("bin"))
        {
            currentDirectory = Environment.CurrentDirectory.Split("bin");
        }
        else
        {
            currentDirectory = Environment.CurrentDirectory;
        }
        string path = Path.Combine(currentDirectory[0], @"CityLatLongDetails\", fileName);

        //Check file exist or not
        if (File.Exists(path))
        {
            string? currentLine;

            using (StreamReader sr = new StreamReader(path))
            {              

                while ((currentLine = sr.ReadLine()) != null)
                {
                    // Search, case insensitive, if the currentLine contains the searched keyword
                    if (currentLine.ToLower().IndexOf(cityName.ToLower(), StringComparison.CurrentCultureIgnoreCase) < 0)
                    {
                        continue;
                    }
                    if (currentLine != null)
                    {
                        Console.WriteLine(currentLine);
                        break;
                    }

                }
            }
                if (!string.IsNullOrEmpty(currentLine))
                {
                    var getCityLatLong = currentLine.Split(",");
                    string Lat = getCityLatLong[1];
                    string Long = getCityLatLong[2];
                    string baseAddress = "https://api.open-meteo.com/v1/forecast";
                    string app = string.Format(baseAddress + "?latitude={0}&longitude={1}&current_weather=true", Lat, Long);
                    Console.WriteLine("\n Please wait..fetching details in the background !! \n");

                    #region call forecast Api
                    using (var client = new HttpClient())
                    {

                        HttpResponseMessage response = await client.GetAsync(app);

                        if (response.IsSuccessStatusCode)
                        {
                            string responsebody = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine(responsebody);


                            Console.WriteLine("\n Please press 'Enter' key for exit....");
                            Console.ReadLine();

                    }
                    else
                    {
                        Console.WriteLine("\n Not able to get data now .Please try after sometime.Please press 'Enter' key for exit....");
                        Console.ReadLine();
                    }
                    }
                    #endregion
                }
            }
        
        else
        {
            Console.WriteLine("provided file path does not exist, file path : {0}", path);
        }
    }
    catch (Exception ex)
    {
        // Log the "ex" exception  to get exception details either use splunk or db log or text  app log

        Console.WriteLine("There is some internal error . Pls. reach out app admin for more detail");
    }
}



