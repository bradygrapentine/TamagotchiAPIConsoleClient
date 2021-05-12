using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;
namespace TamagotchiAPIConsoleClient
{
    public class Pet
    { // add JSON properties
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public int HungerLevel { get; set; }
        public int HappinessLevel { get; set; }
        public DateTime LastInteractedWithDate { get; set; }
        public Boolean IsDead
        {
            get
            {
                if (LastInteractedWithDate.AddDays(3) <= DateTime.Now || HungerLevel >= 15)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string Description()
        {
            return $"{Id}: Name ~ {Name} Happiness Level ~ {HappinessLevel} Hunger Level ~ {HungerLevel} Birthday ~ {Birthday} Last Interaction Date ~ {LastInteractedWithDate} Dead? ~ {IsDead}";
        }
    }
    class Program
    {
        static async Task GetPets()
        {
            var url = "http://localhost:5000/api/Pets";
            var client = new HttpClient();
            var allPetsAsStream = await client.GetStreamAsync(url);
            var allPets = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Pet>>(allPetsAsStream);
            Console.WriteLine();
            foreach (var pet in allPets)
            {
                Console.WriteLine(pet.Description());
            }
            Console.WriteLine();
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------//
        static async Task GetDeadPets()
        {
            var url = "http://localhost:5000/api/Pets?graveyard=true";
            var client = new HttpClient();
            var allPetsAsStream = await client.GetStreamAsync(url);
            var allPets = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Pet>>(allPetsAsStream);
            Console.WriteLine();
            foreach (var pet in allPets)
            {
                Console.WriteLine(pet.Description());
            }
            Console.WriteLine();
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------//
        static async Task PostPet()
        {
            //{"name": "Stevie"} String input for posting
            var client = new HttpClient();
            Console.Write("What is the name of your new pet? ");
            var name = Console.ReadLine();
            var newPet = new Pet();
            newPet.Name = name;
            var jsonBody = JsonSerializer.Serialize(newPet);
            var jsonBodyAsContent = new StringContent(jsonBody);
            jsonBodyAsContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var url = "http://localhost:5000/api/Pets";
            var response = await client.PostAsync(url, jsonBodyAsContent);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------//
        static async Task<Pet> GetPet(string id)
        {
            var client = new HttpClient();
            if (id != null)
            {
                var url = $"http://localhost:5000/api/Pets/{id}";
                var petAsStream = await client.GetStreamAsync(url);
                var pet = await System.Text.Json.JsonSerializer.DeserializeAsync<Pet>(petAsStream);
                return pet;
            }
            else
            {
                return null;
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------//
        static async Task CreatePlaytime()
        {
            Console.Write("What is the id of the pet that you'd like to play with? ");
            var id = Console.ReadLine();
            var client = new HttpClient();
            var newPlaytime = "{}";
            var jsonBody = JsonSerializer.Serialize(newPlaytime);
            var jsonBodyAsContent = new StringContent(jsonBody);
            jsonBodyAsContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var url = $"http://localhost:5000/api/Pets/{id}/Playtimes";
            var response = await client.PostAsync(url, jsonBodyAsContent);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------//
        static async Task CreateFeeding()
        {
            Console.Write("What is the id of the pet that you'd like to feed? ");
            var id = Console.ReadLine();
            var client = new HttpClient();
            var newFeeding = "{}";
            var jsonBody = JsonSerializer.Serialize(newFeeding);
            var jsonBodyAsContent = new StringContent(jsonBody);
            jsonBodyAsContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var url = $"http://localhost:5000/api/Pets/{id}/Feedings";
            var response = await client.PostAsync(url, jsonBodyAsContent);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------//
        static async Task CreateScolding()
        {
            Console.Write("What is the id of the pet that you'd like to scold? ");
            var id = Console.ReadLine();
            var client = new HttpClient();
            var newScolding = "{}";
            var jsonBody = JsonSerializer.Serialize(newScolding);
            var jsonBodyAsContent = new StringContent(jsonBody);
            jsonBodyAsContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var url = $"http://localhost:5000/api/Pets/{id}/Scoldings";
            var response = await client.PostAsync(url, jsonBodyAsContent);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------//
        static async Task DeletePet()
        {
            var client = new HttpClient();
            Console.Write("What is the id of the pet that you'd like to delete? ");
            var id = Console.ReadLine();
            var url = $"http://localhost:5000/api/Pets/{id}";
            var response = await client.DeleteAsync(url);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------//
        static async Task RenamePet()
        {
            //{"id": 14,"name": "Stevie"} String input for renaming
            Console.Write("What is the id of the pet that you'd like to rename? ");
            var id = Console.ReadLine();
            Console.WriteLine();
            Pet updatedPet = await GetPet(id);
            Console.Write("What is the pet's new name? ");
            var name = Console.ReadLine();
            Console.WriteLine();
            var client = new HttpClient();
            updatedPet.Name = name;
            var jsonBody = JsonSerializer.Serialize(updatedPet);
            var jsonBodyAsContent = new StringContent(jsonBody);
            jsonBodyAsContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var url = $"http://localhost:5000/api/Pets/{id}";
            var response = await client.PutAsync(url, jsonBodyAsContent);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------//
        static string Menu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("(SL) See living pets");
            Console.WriteLine("(SD) See dead pets");
            Console.WriteLine("(C)reate new pet");
            Console.WriteLine("(P)lay with a pet (must know pet ID)");
            Console.WriteLine("(F)eed a pet (must know pet ID)");
            Console.WriteLine("(S)cold a pet (must know pet ID)");
            Console.WriteLine("(R)ename a pet(must know pet ID");
            Console.WriteLine("(D)elete a pet(must know pet ID");
            Console.WriteLine("(Q) Quit the application");
            Console.WriteLine();
            Console.Write("Select an option and press Enter: ");
            var choice = Console.ReadLine().ToUpper();
            return choice.ToUpper();
        }
        public static async Task Main(string[] args)
        {

            //------------------------------------------------------------------------------//

            var counter = 0;
            var keepAsking = true;
            while (keepAsking)
            {
                var menuSelection = Menu();
                switch (menuSelection)
                {
                    //------------------------------------------------//
                    case "SL":
                        Console.Clear();
                        await GetPets();
                        Console.WriteLine("");
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    //------------------------------------------------//
                    case "SD":
                        Console.Clear();
                        await GetDeadPets();
                        Console.WriteLine("");
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    //------------------------------------------------//
                    case "C":
                        Console.Clear();
                        await PostPet();
                        Console.WriteLine("");
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    //------------------------------------------------//
                    case "P":
                        Console.Clear();
                        await CreatePlaytime();
                        Console.WriteLine("");
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    //------------------------------------------------//
                    case "F":
                        Console.Clear();
                        await CreateFeeding();
                        Console.WriteLine("");
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    //------------------------------------------------//
                    case "S":
                        Console.Clear();
                        await CreateScolding();
                        Console.WriteLine("");
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    //------------------------------------------------//
                    case "D":
                        Console.Clear();
                        await DeletePet();
                        Console.WriteLine("");
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    //------------------------------------------------//
                    case "R":
                        Console.Clear();
                        await RenamePet();
                        Console.WriteLine("");
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;
                    //------------------------------------------------//
                    case "Q":
                        Console.Clear();
                        Console.WriteLine("Closing application...");
                        keepAsking = false;
                        break;
                    //------------------------------------------------//
                    default:
                        counter += 1;
                        if (counter >= 3)
                        {
                            Console.Clear();
                            Console.WriteLine("Closing application...");
                            keepAsking = false;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Try again");
                            Console.WriteLine();
                            break;
                        }
                        break;
                        //------------------------------------------------//
                }
            }

            //------------------------------------------------------------------------------//

            Console.WriteLine();
            Console.WriteLine("Goodbye");
            Console.WriteLine();

            //------------------------------------------------------------------------------//    }
        }
    }
}
