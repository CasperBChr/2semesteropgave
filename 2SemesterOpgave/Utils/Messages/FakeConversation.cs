using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using _2SemesterOpgave.Models; // Giver adgang til vores modelklasser, fx User og Conversation
using _2SemesterOpgave.Services; // Giver adgang til ConversationServices
using Microsoft.VisualBasic;


namespace _2SemesterOpgave.Utils
{
	// Klasse der simulerer fake samtaler med bot-brugere
	/// <summary>
	/// Kodet af Martin
	/// </summary>
	public class FakeConversation
    {
        // Bruges til at stoppe bot-trådene, når programmet lukker ned
        bool _isShuttingDown = false;

        // Lock bruges til at undgå at flere tråde skriver beskeder samtidig
        static readonly object _lock = new object();

        // Event der kaldes, når der kommer en ny besked
        public event Action<Conversation>? OnNewMessage;

        // Starter fake bots for tilfældige brugere
        public void StartFakeBots(List<User> botUsers, User currentUser, ConversationServices conversationServices)
        {
            // Random bruges til at vælge tilfældige bots og intervaller
            Random rng = new Random();

            // Vælger 3 tilfældige bot-brugere, men ikke den nuværende bruger
            List<User> randomBots = PickRandom(botUsers, currentUser, 3, rng);

            // Gennemgår alle valgte bots
            foreach (User bot in randomBots)
            {
                // Henter eller opretter en samtale mellem currentUser og botten
                Conversation conversation = conversationServices.GetOrCreateConversation(currentUser, bot);

                // Starter botten, så den kan sende beskeder i samtalen
                ContinueConversationBot(conversation, bot, conversationServices, rng.Next(3000, 8000));
            }
        }

        // Starter en ny baggrundstråd til en fake samtale
        public void ContinueConversationBot(Conversation conversation, User botUser, ConversationServices conversationServices, int intervalMs = 5000)
        {
            // Opretter en ny tråd der kører fake samtalen
            Thread thread = new Thread(() =>
            {
                // Starter loopet der sender fake beskeder
                RunFakeConversations(conversation, botUser, conversationServices, intervalMs);
            });

            // Gør tråden til en baggrundstråd, så den ikke holder programmet åbent
            thread.IsBackground = true;

            // Starter tråden
            thread.Start();
        }


        // Vælger et antal tilfældige brugere fra listen
        List<User> PickRandom(List<User> users, User exclude, int count, Random rng)
        {
            // Opretter en liste med mulige brugere
            List<User> pool = new List<User>();

            // Gennemgår alle brugere
            for (int i = 0; i < users.Count; i++)
            {
                // Tilføjer kun brugere der ikke er den bruger der skal udelukkes
                if (users[i].Id != exclude.Id)
                {
                    // Tilføjer brugeren til puljen
                    pool.Add(users[i]);
                }
            }

            // Opretter listen med de valgte brugere
            List<User> result = new List<User>();

            // Fortsætter indtil der er valgt nok brugere, eller puljen er tom
            while (result.Count < count && pool.Count > 0)
            {
                // Vælger et tilfældigt index i puljen
                int index = rng.Next(pool.Count);

                // Tilføjer brugeren til resultatlisten
                result.Add(pool[index]);

                // Fjerner brugeren fra puljen, så samme bruger ikke vælges igen
                pool.RemoveAt(index);
            }

            // Returnerer de tilfældigt valgte brugere
            return result;
        }

        // Kører fake samtalen og sender beskeder med mellemrum
        void RunFakeConversations(Conversation conversation, User botUser, ConversationServices conversationServices, int intervalMs)
        {
            // Random bruges til tilfældige beskeder og intervaller
            Random rng = new Random();

            // Fortsætter så længe programmet ikke lukker ned
            while (!_isShuttingDown)
            {
                // Venter et bestemt antal millisekunder før næste besked
                Thread.Sleep(intervalMs);

                // Låser koden, så kun én tråd sender besked ad gangen
                lock (_lock)
                {
                    // Sender en tilfældig besked fra botten
                    conversationServices.SendMessage(conversation, botUser, RandomMessageText(rng));

                    // Kalder eventet, så UI eller andre dele kan reagere på den nye besked
                    OnNewMessage?.Invoke(conversation);
                }

                // Sætter et nyt tilfældigt interval før næste besked
                intervalMs = rng.Next(3000, 10000);
            }
        }

        // Returnerer en tilfældig beskedtekst
        public string RandomMessageText(Random rng)
        {
            // Liste med mulige fake beskeder
            string[] randomText = new string[]
            {
                "Er den på lager?????",
                "Haaaaallo",
                "Er den small en rigtig small?",
                "Kan du gøre det billigere?",
                "Kan du sende flere billeder????",
                "Hvor hurtigt kan du sende?",
                "Hvor brugt er den?"
            };

            // Returnerer en tilfældig tekst fra listen
            return randomText[rng.Next(randomText.Length)];
        }

        // Returnerer en tilfældig beskedtekst med en ny Random
        public string RandomMessageText() => RandomMessageText(new Random());

        // Stopper fake bot-samtalerne
        public void Shutdown()
        {
            // Sætter shutdown til true, så while-loopet stopper
            _isShuttingDown = true;
        }
    }
}