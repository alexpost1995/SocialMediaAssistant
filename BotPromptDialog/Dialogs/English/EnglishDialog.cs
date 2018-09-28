using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotPromptDialog.Dialogs
{
    [Serializable]
    public class EnglishDialog : IDialog<object>
    {
        string name;
        string age;
        string phone;

        private static Attachment EnglishWelcomeCard()
        {
            var heroCard = new HeroCard
            {
                Title = "Your personal social media assistent",
                Subtitle = "",
                Text = "Hi, I am your personal social media assistant. Ik can help you with the posting of content on your different social media channels.",
                Images = new List<CardImage> { new CardImage("https://d1dh93s7n44ml6.cloudfront.net/blog/wp-content/uploads/2017/04/14095449/my-virtual-assistant.jpg") },
            };
            return heroCard.ToAttachment();
        }

        public enum Topics
        {
            Tech,
            Sport,
            Automotive,
            Formula1,
            Gadgets,
            Phones,
            Computers
        }

        public async Task StartAsync(IDialogContext context)
        {
            //Show the title with background image and Details
            var message = context.MakeMessage();
            var attachment = EnglishWelcomeCard();
            message.Attachments.Add(attachment);
            await context.PostAsync(message);

            PromptDialog.Text(
            context: context,
            resume: ResumeGetName,
            prompt: "Please allow me to ask you a few questions. What is your name?",
            retry: "Sorry, I didn't understand that. Please try again."
            );
        }


        public virtual async Task ResumeGetName(IDialogContext context, IAwaitable<string> Username)
        {
            string response = await Username;
            name = response;
            await context.PostAsync($"Nice to meet you, {name}");

            PromptDialog.Text(
                context: context,
                resume: ResumeShowTopics,
                prompt: $"What is your age, {name}?",
                retry: "Sorry, I didn't understand that. Please try again."
            );
        }

        public virtual async Task ResumeShowTopics(IDialogContext context, IAwaitable<string> Age)
        {
            string response = await Age;
            age = response;
            await context.PostAsync($"Your name is {name} and you are {age} years old.");
            context.Wait(this.TopicsPrompt);

            //PromptDialog.Text(
            //    context: context,
            //    resume: TopicsPrompt,
            //    prompt: $"Over welke topics zou je iets op social media willen posten",
            //    retry: "Sorry, I didn't understand that. Please try again."
            //);
        }

        public virtual async Task TopicsPrompt(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var message = await activity;

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: (IEnumerable<Topics>)Enum.GetValues(typeof(Topics)),
                prompt: "What topic would you like to post a message about on social media?",
                retry: "Please select one of the options above",
                promptStyle: PromptStyle.Auto
                );
        }

        public virtual async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<Topics> activity)
        {
            Topics response = await activity;
            if (response.ToString() == "Nederlands")
            {
                //context.Call<object>(new DutchDialog(response.ToString()), ChildDialogComplete);

            }
            if (response.ToString() == "English")
            {
                //context.Call<object>(new EnglishDialog(response.ToString()), ChildDialogComplete);

            }
            else
            {
                await context.PostAsync("abc");
            }

        }


        public virtual async Task ResumeGetPhone(IDialogContext context, IAwaitable<string> mobile)
        {
            string response = await mobile;
            phone = response;

            await context.PostAsync(String.Format("Hello {0} ,Congratulation :) Your  C# Corner Annual Conference 2018 Registrion Successfullly completed with Name = {0}  Mobile Number {2} . You will get Confirmation email and SMS", name, phone));

            context.Done(this);
        }
    }
}