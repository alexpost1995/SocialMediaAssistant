using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BotPromptDialog.Dialogs
{
    [Serializable]
    public class DutchDialog : IDialog<object>
    {
        string name;
        string age;
        string phone;
        string welcomeCardTitle = "Je persoonlijke social media assistent";

        private static Attachment DutchWelcomeCard()
        {
            var heroCard = new HeroCard
            {
                Title = "Je persoonlijke social media assistent",
                Subtitle = "",
                Text = "Hi, ik ben je persoonlijke social media assistent. Ik kan je helpen met het plaatsen van content op je social media kanalen",
                Images = new List<CardImage> { new CardImage("https://d1dh93s7n44ml6.cloudfront.net/blog/wp-content/uploads/2017/04/14095449/my-virtual-assistant.jpg") },
            };
            return heroCard.ToAttachment();
        }

        private static IList<Attachment> GetCardsAttachments()
        {
            return new List<Attachment>()
            {
                GetHeroCard(
                    "Tech",
                    "Facebook steekt 1 miljard dollar in eerste Aziatische datacenter",
                    "Sociaal netwerk Facebook investeert 1 miljard dollar in de bouw van een nieuw datacenter in Singapore, het eerste van het bedrijf in Azië.",
                    new CardImage(url: "https://media.nu.nl/m/er1xhn4apjxh_wd640.jpg/facebook-steekt-1-miljard-dollar-in-eerste-aziatische-datacenter.jpg"),
                    new CardAction(ActionTypes.OpenUrl, "Bekijk artikel", value: "https://www.nu.nl/internet/5449418/facebook-steekt-1-miljard-dollar-in-eerste-aziatische-datacenter.html")),
                GetHeroCard(
                    "Formule 1",
                    "FIA bespreekt DRS-problemen met teams na crash Ericsson",
                    "FIA Formule 1-wedstrijdleider Charlie Whiting bespreekt donderdag de gevolgen van de DRS-problemen en de crash van Marcus Ericsson in een bijeenkomst met de technisch directeuren van alle F1-teams.",
                    new CardImage(url: "https://adn.gpupdate.net/news/322753.jpg"),
                    new CardAction(ActionTypes.OpenUrl, "Bekijk", value: "https://www.gpupdate.net/nl/f1-nieuws/367125/fia-bespreekt-drs-problemen-met-teams-na-crash-ericsson/")),
                GetHeroCard(
                    "Gadgets",
                    "Zelfrijdende auto van Apple betrokken bij ongeluk",
                    "Apple heeft een incident met een zelfrijdende auto gemeld bij de toezichthouder, het Department of Motor Vehicles (DMV) in de Amerikaanse staat Californië.",
                    new CardImage(url: "https://media.nu.nl/m/zauxd20a10od_wd640.jpg/zelfrijdende-auto-van-apple-betrokken-bij-ongeluk.jpg"),
                    new CardAction(ActionTypes.OpenUrl, "Bekijk artikel", value: "https://www.nu.nl/gadgets/5442242/zelfrijdende-auto-van-apple-betrokken-bij-ongeluk.html")),
                GetHeroCard(
                    "Mobiel",
                    "Nieuwe iPhones krijgen geen vingerafdrukscanner achter scherm",
                    "Apple is voorlopig niet van plan iPhones met een vingerafdrukscanner achter het scherm uit te brengen.",
                    new CardImage(url: "https://media.nu.nl/m/vgcxrdca5hxq_wd640.jpg/nieuwe-iphones-krijgen-geen-vingerafdrukscanner-achter-scherm.jpg"),
                    new CardAction(ActionTypes.OpenUrl, "Bekijk artikel", value: "https://www.nu.nl/mobiel/5447741/nieuwe-iphones-krijgen-geen-vingerafdrukscanner-achter-scherm.html")),
                GetVideoCard(),
            };
        }

        private static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetVideoCard()
        {
            var videoCard = new VideoCard
            {
                Title = "Big Buck Bunny",
                Subtitle = "by the Blender Institute",
                Text = "Big Buck Bunny (code-named Peach) is a short computer-animated comedy film by the Blender Institute, part of the Blender Foundation. Like the foundation's previous film Elephants Dream, the film was made using Blender, a free software application for animation made by the same foundation. It was released as an open-source film under Creative Commons License Attribution 3.0.",
                Image = new ThumbnailUrl
                {
                    Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c5/Big_buck_bunny_poster_big.jpg/220px-Big_buck_bunny_poster_big.jpg"
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "http://download.blender.org/peach/bigbuckbunny_movies/BigBuckBunny_320x180.mp4"
                    }
                },
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Title = "Learn More",
                        Type = ActionTypes.OpenUrl,
                        Value = "https://peach.blender.org/"
                    }
                }
            };

            return videoCard.ToAttachment();
        }

        public enum Topics
        {
            Tech,
            Sport,
            Automotive,
            [Display(Name = "Formule 1")]
            Formule1,
            [Display(Name = "Vrije tijd")]
            VrijeTijd,
            Gadgets
        }

        public async Task StartAsync(IDialogContext context)
        {
            //Show the title with background image and Details
            var message = context.MakeMessage();
            var attachment = DutchWelcomeCard();
            message.Attachments.Add(attachment);
            await context.PostAsync(message);

            PromptDialog.Text(
            context: context,
            resume: ResumeGetName,
            prompt: "Laat me je een paar vragen stellen. Wat is je naam?"
            );
        }        
        
        public virtual async Task ResumeGetName(IDialogContext context, IAwaitable<string> Username)
        {
            string response = await Username;
            name = response;
            await context.PostAsync($"Aangenaam, {name}");

            //AdaptiveCard card = new AdaptiveCard()
            //{
            //    Body = new List<CardElement>()
            //    {
            //        new Container()
            //        {
            //            Items = new List<CardElement>()
            //            {
            //                new ColumnSet()
            //                {
            //                    Columns = new List<Column>()
            //                    {
            //                        new Column()
            //                        {
                                        
            //                            Size = ColumnSize.Stretch,
            //                            Items = new List<CardElement>()
            //                            {
            //                                new TextBlock()
            //                                {
            //                                    Text =  "VS klaagt Koreaanse hacker aan voor WannaCry en Sony-hack",
            //                                    Weight = TextWeight.Bolder,
            //                                    IsSubtle = false,
            //                                },
            //                                new Image()
            //                                {
            //                                    Url = "https://media.nu.nl/m/m1oxjdva9335_wd640.jpg/vs-klaagt-koreaanse-hacker-wannacry-en-sony-hack.jpg",
            //                                    Size = ImageSize.Large,
            //                                    Style = ImageStyle.Normal
            //                                },
            //                                new TextBlock()
            //                                {
            //                                    Text = "De Verenigde Staten hebben een Noord-Koreaanse hacker aangeklaagd voor de aanval met de gijzelsoftware WannaCry in 2017, aanvallen op de centrale bank van Bangladesh in 2016 en de hack op Sony in 2014.",
            //                                    Weight = TextWeight.Normal,
            //                                    IsSubtle = true
            //                                },
            //                                new TextBlock()
            //                                {
            //                                    Text = "Functie",
            //                                    Weight = TextWeight.Normal,
            //                                    IsSubtle = false
            //                                },new DateInput()
            //                                {
            //                                    Id = "Checkin",
            //                                    Speak = "<s>When do you want to check in?</s>"
            //                                },
            //                                new TextBlock()
            //                                {
            //                                    Text = "Are you looking for a flight or a hotel?",
            //                                    Wrap = true
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //};

            //Attachment attachment = new Attachment()
            //{
            //    ContentType = AdaptiveCard.ContentType,
            //    Content = card
            //};

            //var reply = context.MakeMessage();
            //reply.Attachments.Add(attachment);

            //await context.PostAsync(reply, CancellationToken.None);

            PromptDialog.Text(
                context: context,
                resume: ResumeShowTopics,
                prompt: $"Wat is je leeftijd, {name}?",
                retry: "Sorry, I didn't understand that. Please try again."
            );
        }

        public virtual async Task TopicsOrSuggestions(IDialogContext context, IAwaitable<string> Age)
        {
            string response = await Age;
            age = response;
            await context.PostAsync($"Okay {name}, ik heb hier een aantal artikelen die je misschien interessant zou vinden om te posten op social media");
            //context.Wait(this.TopicsPrompt);
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCardsAttachments();

            await context.PostAsync(reply);

            //PromptDialog.Text(
            //    context: context,
            //    resume: TopicsPrompt,
            //    prompt: $"Over welke topics zou je iets op social media willen posten",
            //    retry: "Sorry, I didn't understand that. Please try again."
            //);
        }

        public virtual async Task ResumeShowTopics(IDialogContext context, IAwaitable<string> Age)
        {
            string response = await Age;
            age = response;
            await context.PostAsync($"Okay {name}, ik heb hier een aantal artikelen die je misschien interessant zou vinden om te posten op social media");
            context.Wait(this.TopicsPrompt);
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCardsAttachments();

            await context.PostAsync(reply);

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
                prompt: "Over welk topic zou je een bericht willen posten?",
                retry: "Kies een van de bovenstaande opties",
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