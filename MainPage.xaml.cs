using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace JokeApp;

public partial class MainPage : ContentPage
{
    string joke = "";
    string[] categoriesList = new string[6];

	public MainPage()
	{
		InitializeComponent();
	}

    private void OnCheckAllCheckBoxChanged(object sender, EventArgs e)
    {
        if (CheckAllCheckBox.IsChecked)
        {
            programmingCheckBox.IsChecked = true;
            darkCheckBox.IsChecked = true;
            miscCheckBox.IsChecked = true;
            punCheckBox.IsChecked = true;
            spookyCheckBox.IsChecked = true;
            christmasCheckBox.IsChecked = true;
        }
        else if(!CheckAllCheckBox.IsChecked)
        {
            programmingCheckBox.IsChecked = false;
            darkCheckBox.IsChecked = false;
            miscCheckBox.IsChecked = false;
            punCheckBox.IsChecked = false;
            spookyCheckBox.IsChecked = false;
            christmasCheckBox.IsChecked = false;
        }
    }

    private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (programmingCheckBox.IsChecked) categoriesList[0] = "Programming";
        else categoriesList[0] = "";

        if (darkCheckBox.IsChecked) categoriesList[1] = "Dark";
        else categoriesList[1] = "";

        if (miscCheckBox.IsChecked) categoriesList[2] = "Misc";
        else categoriesList[2] = "";

        if (punCheckBox.IsChecked) categoriesList[3] = "Pun";
        else categoriesList[3] = "";

        if (spookyCheckBox.IsChecked) categoriesList[4] = "Spooky";
        else categoriesList[4] = "";

        if (christmasCheckBox.IsChecked) categoriesList[5] = "Christmas";
        else categoriesList[5] = "";

        if (programmingCheckBox.IsChecked &&
            darkCheckBox.IsChecked &&
            miscCheckBox.IsChecked &&
            punCheckBox.IsChecked &&
            spookyCheckBox.IsChecked &&
            christmasCheckBox.IsChecked)
        {
            CheckAllCheckBox.IsChecked = true;
        }
    }

    private async void OnJokeButtonClick(object sender, EventArgs e)
    {
        List<string> list = new List<string>();
        foreach(string category in categoriesList) 
        { 
            if(!string.IsNullOrEmpty(category)) list.Add(category); 
        }
        string categories = string.Join(",", list.Take(list.Count));
        if (string.IsNullOrEmpty(categories)) categories = "Any";

        var client = new HttpClient();
        var response = await client.GetAsync($"https://v2.jokeapi.dev/joke/{categories}?blacklistFlags=racist,sexist");
        var jokeContent = await response.Content.ReadAsStringAsync();
        var jokeJson = JObject.Parse(jokeContent);

        if (jokeJson["type"].ToString() ==  "twopart") 
        {
            var setup = jokeJson["setup"].ToString() + "\n";
            var delivery = jokeJson["delivery"].ToString();
            jokeTxt.Text = setup + delivery;
            SemanticScreenReader.Announce(jokeTxt.Text);
        }
        else
        {
            jokeTxt.Text = jokeJson["joke"].ToString();
		    SemanticScreenReader.Announce(jokeTxt.Text);
        }
    }
}

