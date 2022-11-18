using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using System.Runtime.InteropServices;

namespace AldioumaBot
{
	class Program
	{
		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

#pragma warning disable CS8618
		public static DiscordSocketClient Client { get; set; }
#pragma warning restore

		public const ulong DlscordBase = 786009481246277653;
		public const ulong Crozza = 1040658160689631302;
		public const ulong TestServer = 1017448373776814120;

		public delegate Task Command(); // per GetCommands()

		public static async Task Main()
		{
			AllocConsole();
			ShowWindow(GetConsoleWindow(), 0);

			Client = new();
			await Client.LoginAsync(TokenType.Bot, Token.DiscordToken);
			await Client.StartAsync();

			Client.Ready += SetUpBot;

			await Task.Delay(-1);
		}

		private static async Task SetUpBot()
		{
			Client.MessageReceived += HandleChatMessage; // quando viene mandato un messaggio in chat
			Client.UserVoiceStateUpdated += HandleUserVoiceState; // quando qualcuno in vocale viene inculato
			Client.SlashCommandExecuted += HandleSlashCommand; // quando viene eseguito /summon

			SlashCommandBuilder summon = new();
			summon.WithName("summon")
				.WithDescription("per quando qualcuno ha l'ebola")
				.WithDMPermission(false)
				.AddOption("cancro", ApplicationCommandOptionType.User, "il cancro da evocare");

			await Client.CreateGlobalApplicationCommandAsync(summon.Build());
		}

		private static async Task HandleChatMessage(SocketMessage arg)
		{
			SocketUserMessage message = (SocketUserMessage)arg;
			SocketCommandContext context = new(Client, message);

			if (message.Author.IsBot) return;
			if (context.IsPrivate)
			{
				if (context.User.Id == 566905651310362656)
				{
					switch (message.Content)
					{
						case "show":
							ShowWindow(GetConsoleWindow(), 5);

							#region CODICE DISUMANO DA NON GUARDARE SE NON PER QUANDO C'È DA FUCILARE GLI IRACHENI
#pragma warning disable CS8629
							string msg = string.Empty;
							await foreach (var auditLogs in Client.GetGuild(786009481246277653).GetAuditLogsAsync(100))
							{
								foreach (var audit in auditLogs)
								{
									msg += $"{audit.User.Username} {audit.Action} ";

									string data = string.Empty;
									switch (audit.Action)
									{
										case ActionType.Ban:
											data = ((BanAuditLogData)audit.Data).Target.Username;
											break;
										case ActionType.Kick:
											data = ((KickAuditLogData)audit.Data).Target.Username;
											break;
										case ActionType.MemberRoleUpdated:
											data = ((MemberRoleAuditLogData)audit.Data).Target.Username;
											foreach (var role in ((MemberRoleAuditLogData)audit.Data).Roles)
											{
												data += $" {(role.Added ? "+" : "-")}{role.Name}";
											}
											break;
										case ActionType.RoleCreated:
											data = ((RoleCreateAuditLogData)audit.Data).Properties.Name;
											if (((GuildPermissions)((RoleCreateAuditLogData)audit.Data).Properties.Permissions).Administrator)
											{
												data += " ADMIN";
											}
											break;
										case ActionType.RoleUpdated:
											var before = ((RoleUpdateAuditLogData)audit.Data).Before;
											var after = ((RoleUpdateAuditLogData)audit.Data).After;
											data += Client.GetGuild(786009481246277653).GetRole(((RoleUpdateAuditLogData)audit.Data).RoleId).Name;
											if (((GuildPermissions)before.Permissions).Administrator && !((GuildPermissions)after.Permissions).Administrator)
											{
												data += " -ADMIN";
											}
											if (!((GuildPermissions)before.Permissions).Administrator && ((GuildPermissions)after.Permissions).Administrator)
											{
												data += " +ADMIN";
											}
											break;
										case ActionType.Unban:
											data = ((UnbanAuditLogData)audit.Data).Target.Username;
											break;
										default: break;
									}

									msg += data + "\n";
								}
#pragma warning restore
							}
							Console.Write(msg);
							#endregion

							break;
						case "hide":
							Console.Clear();
							ShowWindow(GetConsoleWindow(), 0);
							break;
						default:
							break;
					}
				}
				return;
			}

			SocketTextChannel ch = (SocketTextChannel)message.Channel;

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
					async () =>
					{
						DateTime attentato = new(2023, 8, 16, 15, 32, 00);
						if (DateTime.Now > attentato) // dopo il 16/8?
							await ch.SendMessageAsync($"il duomo è gia esploso {Math.Floor((DateTime.Now - attentato).TotalDays)} giorni fa 🙏🏿");
						else if (DateTime.Now < attentato) // prima del 16/8?
							await ch.SendMessageAsync($"mancano {Math.Floor((DateTime.Now - attentato).TotalDays * -1)} giorni all'attentato al duomo di pisa 🙏🏿");
						else // è il 16/8?
							await ch.SendMessageAsync("oggi è il grande giorno 🙏🏿 preparate le bombe");
					},
					async () => await ch.SendMessageAsync("dio stronzo mi sto a segà e questo tagga tutti"),
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

		private static async Task HandleUserVoiceState(SocketUser user, SocketVoiceState before, SocketVoiceState after)
		{
			try
			{
				if (
					user.IsBot // bot
				|| before.VoiceChannel is null // join
				|| after.VoiceChannel is null // leave
				|| (before.VoiceChannel.Id == after.VoiceChannel.Id) // non si è spostato
				|| (before.VoiceChannel.Guild.Id != 786009481246277653) // non in dlscord base
				) return;

				var channel = Client.GetGuild(DlscordBase).GetTextChannel(1016799894335410187);

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
				if (command.User.Id == 920346045135872020) // bambino piccolo che di cognome fa appiani?
				{
					await command.RespondAsync("dio porco ti chiami francesco appiani e pensi di meritarti qualcosa da un bot");
					return;
				}
				else if (((SocketGuildUser)command.User).Guild.Id == DlscordBase && !((SocketGuildUser)command.User).Roles.Contains(Client.GetGuild(DlscordBase).GetRole(786009704270004228))) // dlscord | presidente?
				{
					await command.RespondAsync("non hai il ruolo presidente quindi non conti un cazzo");
					return;
				}
				else if (((SocketGuildUser)command.User).Guild.Id == Crozza && !((SocketGuildUser)command.User).Roles.Contains(Client.GetGuild(DlscordBase).GetRole(1040741716547866704))) // crozza | twitch?
				{
					await command.RespondAsync("non hai il ruolo signor. twitch quindi non conti un cazzo");
					return;
				}

				SocketGuildUser cancro = (SocketGuildUser)command.Data.Options.ElementAt(0).Value;
				string name = cancro.Username;
				string discriminator = cancro.Discriminator;

				if (cancro.VoiceChannel is null)
				{
					await command.RespondAsync("dio negro non è in chat vocale");
					return;
				}
				if (cancro.Id == 1016759379028095047)
				{
					await command.RespondAsync("https://imgur.com/a/nKG1yGp");
					return;
				}

				await command.RespondAsync(":ok_hand:");

				SocketVoiceChannel channel = cancro.VoiceChannel;
				foreach (var ch in Client.GetGuild((ulong)command.GuildId!).VoiceChannels.OrderByDescending(ch => ch.Position))
				{
					try
					{
						if (ch.Id == channel.Id || ch.ConnectedUsers.Count > 0) continue;
						await cancro.ModifyAsync(user => user.Channel = ch);
						await Task.Delay(250);
					}
					catch (HttpException) { } // carcere di twitter
				}
				await cancro.ModifyAsync(user => user.Channel = channel);
			}
		}
	}
}
