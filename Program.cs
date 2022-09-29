using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace AldioumaBot
{
	class Program
	{
#pragma warning disable CS8618
		public static DiscordSocketClient Client { get; set; }
#pragma warning restore

		public const ulong GuildId = 786009481246277653;
		// 786009481246277653 dlscord base
		// 1017448373776814120 test

		public delegate Task Command(); // per GetCommands()

		public static async Task Main()
		{
			Client = new();
			await Client.LoginAsync(TokenType.Bot, Token.DiscordToken);
			await Client.StartAsync();

			Client.Ready += SetUpBot;
			
			await Task.Delay(-1);
		}

		private static async Task SetUpBot()
		{
			Client.MessageReceived += HandleChatMessage; // quando viene mandato un messaggio in chat
			Client.UserVoiceStateUpdated += HandleUserState; // quando qualcuno in vocale viene inculato
			Client.SlashCommandExecuted += HandleSlashCommand; // quando viene eseguito /summon
			Client.UserJoined += HandleUserJoin; // se brando aggiungerà la segregazione razziale

			SlashCommandBuilder summon = new();
			summon.WithName("summon")
				.WithDescription("per quando qualcuno ha l'ebola")
				.WithDMPermission(false)
				.AddOption("cancro", ApplicationCommandOptionType.User, "il cancro da evocare");

			await Client.CreateGlobalApplicationCommandAsync(summon.Build());
		}

		private static async Task HandleChatMessage(SocketMessage arg)
		{
			if (arg is not SocketUserMessage) return;

			SocketUserMessage message = (SocketUserMessage)arg;
			SocketCommandContext context = new(Client, message);
			SocketTextChannel ch = (SocketTextChannel)message.Channel;

			if (message.Author.IsBot) return;
			if (context.IsPrivate) return;

			for (int i = 0; i < GetKeywords().Length; i++)
			{
				if (message.Content.Contains(GetKeywords()[i]))
				{
					await GetCommands()[i]();
				}
			}

			static string[] GetKeywords()
			{
				return new string[]
				{
					"duomo",
					"@everyone",
					"kebe",
					"oggettiv",
					"bersaglio",
					"bombe",
					"attentato",
					"allah",
					"kabobo",
					"matteo",
					"1984",
					"avigliano",
					"cyberpunk"
				};
			}

			Command[] GetCommands()
			{
				return new Command[]
				{
					async () => await ch.SendMessageAsync($"il duomo è gia esploso {Math.Floor((DateTime.Now - new DateTime(2021, 8, 16, 15, 32, 0)).TotalDays)} giorni fa 🙏🏿"),
					async () =>	await ch.SendMessageAsync("dio stronzo mi sto a segà e questo tagga tutti"),
					async () => await ch.SendMessageAsync("🙏🏿 goudi kebe akbar mihail touba sawarim islam jihad pontedera allah aldiouma 🙏🏿"),
					async () => await ch.SendMessageAsync("https://imgur.com/a/eLS0Flt"),
					async () => await ch.SendMessageAsync("https://imgur.com/a/XIMVXEg"),
					async () => await ch.SendMessageAsync("https://imgur.com/a/QcmIdN0"),
					async () => await ch.SendMessageAsync("https://imgur.com/a/gHX8tVP"),
					async () => await ch.SendMessageAsync("https://imgur.com/a/k810KJc"),
					async () => await ch.SendMessageAsync("https://imgur.com/a/zMIeZ9N"),
					async () => await ch.SendMessageAsync("https://imgur.com/a/umENPRL"),
					async () => await ch.SendMessageAsync(Helpers.Get1984()),
					async () => await ch.SendMessageAsync("è arrivata mammina la piscia r ta mamma scrivi scrivi :grin: si sol n'abbunat"),
					async () => await ch.SendMessageAsync("https://www.youtube.com/watch?v=qUxBn5KDxsA ووكدسكاكپوسداوكيدجخجفدسفيس")
				};
			}
		}

		private static async Task HandleUserState(SocketUser user, SocketVoiceState before, SocketVoiceState after)
		{
			try
			{
				// ritornerà false solo se user (si) è spostato da un canale all'altro
				if (
					user.IsBot // è un bot
				||	before.VoiceChannel is null // user join
				||  after.VoiceChannel is null // user leave
				||  (before.VoiceChannel.Id == after.VoiceChannel.Id) // user mute/deafen/stream/cam
				) return;

				var channel = Client.GetGuild(GuildId).GetTextChannel(1016799894335410187);

				EmbedBuilder embed = new();
				embed.WithAuthor("MUHAMED KEBE")
					.WithColor(Discord.Color.Blue)
					.WithImageUrl("https://i.imgur.com/CDUriO4.jpg")
					.WithThumbnailUrl("https://i.imgur.com/CylNIQY.png")
					.WithFooter("A MENO CHE TU NON SIA IN COMA VEDI DI ENTRARE IN UN CANALE");

				await channel.SendMessageAsync($"{user.Mention} WAKE UP  WAKE UP  WAKE UP  WAKE UP  WAKE UP  WAKE UP  WAKE UP", embed: embed.Build());
			}
			catch
			{
				// solo quando è in corso HandleSlashCommand(), oppure 1 volta su 500

				// https://it.wikipedia.org/wiki/Algoritmo_dello_struzzo
			}
		}

		private static async Task HandleSlashCommand(SocketSlashCommand command)
		{
			if (command.CommandName == "summon")
			{
				if (!((SocketGuildUser)command.User).Roles.Contains(Client.GetGuild(GuildId).GetRole(786009704270004228))) // presidente?
				{
					await command.RespondAsync("non hai il ruolo presidente quindi non conti un cazzo");
					return;
				}

				SocketGuildUser cancro = (SocketGuildUser)command.Data.Options.ElementAt(0).Value;
				string name = cancro.Username;
				string discriminator = cancro.Discriminator;
			
				if (cancro.IsBot)
				{
					await command.RespondAsync("dio negro è un bot");
					return;
				}
				if (cancro.VoiceChannel is null)
				{
					await command.RespondAsync("dio negro non è in chat vocale");
					return;
				}
			
				await command.RespondAsync(":ok_hand:");

				SocketVoiceChannel channel = cancro.VoiceChannel;
				foreach (var ch in Client.GetGuild(GuildId).VoiceChannels.OrderByDescending(ch => ch.Position))
				{ 
					if (ch.Id == channel.Id) continue;
					await cancro.ModifyAsync(user => user.Channel = ch);
					await Task.Delay(250);
				}
				await cancro.ModifyAsync(user => user.Channel = channel);
			}
		}

		private static async Task HandleUserJoin(SocketGuildUser user)
		{
			return; // da togliere se brando aggiunge la segregazione razziale
#pragma warning disable CS0162 // DIO BOIA MUORI
			string[] negri =
			{
				"Pippolo06",
				"PitOscuro06",
				"Guardian",
				"PTR_josh"
			};

			if (negri.Contains(user.Username))
			{
				await user.AddRoleAsync(0 /* ruolo che toglie ogni diritto ai tumori */);
			}
#pragma warning restore
		}
	}
}