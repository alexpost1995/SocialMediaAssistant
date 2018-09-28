using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace BotPromptDialog.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi, welcome!");
            context.Wait(this.LanguagePrompt);
        }

        public virtual async Task LanguagePrompt(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var message = await activity;

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: (IEnumerable<Language>)Enum.GetValues(typeof(Language)),
                prompt: "Selecteer uw taal | Please select your language",
                retry: "Please choose one of the above languages",
                promptStyle: PromptStyle.Auto
                );
        }

        public virtual async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<Language> activity)
        {
            Language response = await activity;
            if (response.ToString() == "Nederlands")
            {
                context.Call<object>(new Dialogs.DutchDialog(), ChildDialogComplete);
            }
            if (response.ToString() == "English")
            {
                context.Call<object>(new Dialogs.EnglishDialog(), ChildDialogComplete);
            }
        }

        public virtual async Task ChildDialogComplete(IDialogContext context, IAwaitable<object> response)
        {
            context.Done(this);
        }

        public enum Language
        {
            Nederlands,
            English
        }
    }
}