using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static AldioumaBot.Helpers;

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

		#region constants
		// windows api
		public const int WINDOW_HIDE = 0;
		public const int WINDOW_SHOW = 5;
		// guild ids
		public const ulong DlscordBase = 786009481246277653;
		public const ulong Crozza = 1040658160689631302;
		public const ulong TestServer = 1017448373776814120;
		// user ids
		public const ulong PareId = 566905651310362656;
		public const ulong SpaceId = 920346045135872020;
		public const ulong AldioumaId = 1016759379028095047;
		public const ulong VanniBotId = 867526392156258324;
		// channel ids
		public const ulong WakeUpId = 1016799894335410187;
        // role ids
        public static IRole? Presidente { get; set; }
		public static IRole? STwitch { get; set; }
		// HandleChatMessage
		public readonly static DateTime Attentato = new(2023, 8, 16, 15, 32, 00);
		public delegate Task Command();
		// altra merda
		public const string ResourcePath = @"C:\Users\Jacopo\Desktop\aldiouma\";
        #endregion // constants

        public static async Task Main()
		{
			AllocConsole();
			ShowWindow(GetConsoleWindow(), WINDOW_HIDE);
			Console.OutputEncoding = Encoding.Unicode;
			SetKeyWords();
			
			Client = new();
			await Client.LoginAsync(TokenType.Bot, Token.DiscordToken);
			await Client.StartAsync();

			Client.Ready += SetUpBot;

			await Task.Delay(Timeout.Infinite);
		}

		private static async Task SetUpBot()
		{
			Client.MessageReceived += HandleChatMessage; // quando viene mandato un messaggio in chat
			Client.UserVoiceStateUpdated += HandleUserVoiceState; // quando qualcuno in vocale viene inculato
			Client.SlashCommandExecuted += HandleSlashCommand; // quando viene eseguito /summon

			#region static setters
			Presidente = Client.GetGuild(DlscordBase).GetRole(786009704270004228);
			STwitch = Client.GetGuild(DlscordBase).GetRole(1040741716547866704);
			#endregion

			#region command builders
			SlashCommandBuilder summon = new();
			summon.WithName("summon")
				.WithDescription("per quando qualcuno ha l'ebola")
				.WithDMPermission(false)
				.AddOption("cancro", ApplicationCommandOptionType.User, "il cancro da evocare", true);

			SlashCommandBuilder rating = new();
			rating.WithName("rating")
				.WithDescription("fa il cock rating")
				.WithDMPermission(false)
				.AddOption("pisello", ApplicationCommandOptionType.User, "persona a cui fare il cock rating", true);
			#endregion

			await Client.CreateGlobalApplicationCommandAsync(summon.Build());
			await Client.CreateGlobalApplicationCommandAsync(rating.Build());
		}

		private static async Task HandleChatMessage(SocketMessage arg)
		{
			SocketUserMessage message = (SocketUserMessage)arg;
			SocketCommandContext context = new(Client, message);

			if (message.Author.IsBot) return;
			if (context.IsPrivate)
			{
				if (context.User.Id == PareId)
				{
					switch (message.Content)
					{
						case "show":
							ShowWindow(GetConsoleWindow(), WINDOW_SHOW);

							#region CODICE DISUMANO DA NON GUARDARE SE NON PER QUANDO C'È DA FUCILARE GLI IRACHENI
#pragma warning disable CS8629
							string msg = string.Empty;
							ulong guild = Crozza;
							await foreach (var auditLogs in Client.GetGuild(guild).GetAuditLogsAsync(100))
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
											data += Client.GetGuild(guild).GetRole(((RoleUpdateAuditLogData)audit.Data).RoleId).Name;
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
							ShowWindow(GetConsoleWindow(), WINDOW_HIDE);
							break;
						default:
							break;
					}
				}
				return;
			}

			SocketTextChannel ch = (SocketTextChannel)message.Channel;

			for (int i = 0; i < KeyWords.Length; i++)
			{
				if (message.Content.Contains(KeyWords[i]))
				{
					await GetCommands()[i]();
				}
			}

			Command[] GetCommands()
			{
				return new Command[]
				{
					async () =>
					{
						double timeDiff = (DateTime.Now - Attentato).TotalDays;
						if (DateTime.Now > Attentato) // dopo il 16/8?
							await ch.SendMessageAsync($"il duomo è gia esploso {Math.Floor(timeDiff)} giorni fa 🙏🏿");
						else if (DateTime.Now < Attentato) // prima del 16/8?
							await ch.SendMessageAsync($"mancano {Math.Floor(timeDiff * -1)} giorni all'attentato al duomo di pisa 🙏🏿");
						else // è il 16/8?
							await ch.SendMessageAsync("oggi è il grande giorno 🙏🏿 preparate le bombe");
					},
					async () => await ch.SendMessageAsync("dio stronzo mi sto a segà e questo tagga tutti"),
					async () => await ch.SendMessageAsync("🙏🏿 goudi kebe akbar mihail touba sawarim islam jihad pontedera allah aldiouma 🙏🏿"),
					async () => await ch.SendFileAsync($"{ResourcePath}oggettività.jpeg", ""),
					async () => await ch.SendFileAsync($"{ResourcePath}duomo.png", ""),
					async () => await ch.SendFileAsync($"{ResourcePath}bombe.jpeg", ""),
					async () => await ch.SendFileAsync($"{ResourcePath}beirut.gif", ""),
					async () => await ch.SendFileAsync($"{ResourcePath}kebe isis.png", ""),
					async () => await ch.SendFileAsync($"{ResourcePath}senegalese.jpeg", ""),
					async () => await ch.SendFileAsync($"{ResourcePath}sbrana duce.png", ""),
					async () => await ch.SendMessageAsync(Get1984()),
					async () => await ch.SendMessageAsync("è arrivata mammina la piscia r ta mamma scrivi scrivi :grin: si sol n'abbunat"),
					async () => await ch.SendMessageAsync("https://www.youtube.com/watch?v=qUxBn5KDxsA ووكدسكاكپوسداوكيدجخجفدسفيس"),
					async () =>
					{
						string msg = string.Empty;
						msg += "comandi:\n/summon\n/rating\n\n";
						msg += "parole sus: ";
						foreach (string cmd in KeyWords)
						{
							if (cmd == "aiuto") continue;
							msg += $"{cmd}, ";
						}
						msg = msg.Replace("@", "@|").Remove(msg.Length - 1);
						await ch.SendMessageAsync(msg);
					}
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
				|| (before.VoiceChannel.Guild.Id != DlscordBase) // non in dlscord base
				) return;

				var channel = Client.GetGuild(DlscordBase).GetTextChannel(WakeUpId);

				EmbedBuilder embed = new();
				embed.WithAuthor("MUHAMED KEBE")
					.WithColor(Discord.Color.Blue)
					.WithImageUrl($"attachment://{ResourcePath}wake up.jpg")
					.WithThumbnailUrl($"attachment://{ResourcePath}corti.png")
					.WithFooter("A MENO CHE TU NON SIA IN COMA VEDI DI ENTRARE IN UN CANALE");

				await channel.SendMessageAsync($"{user.Mention} WAKE UP  WAKE UP  WAKE UP  WAKE UP  WAKE UP  WAKE UP  WAKE UP", embed: embed.Build());
			}
			catch { } // se è eseguito /summon
		}

		private static async Task HandleSlashCommand(SocketSlashCommand command)
		{
			if (command.CommandName == "summon")
			{
				if (command.User.Id == SpaceId) // bambino piccolo che di cognome fa appiani?
				{
					await command.RespondAsync("dio porco ti chiami francesco appiani e pensi di meritarti qualcosa da un bot");
					return;
				}
				else if (((SocketGuildUser)command.User).Guild.Id == DlscordBase && !((SocketGuildUser)command.User).Roles.Contains(Presidente)) // dlscord | presidente?
				{
					await command.RespondAsync("non hai il ruolo presidente quindi non conti un cazzo");
					return;
				}
				else if (((SocketGuildUser)command.User).Guild.Id == Crozza && ((SocketGuildUser)command.User).Roles.Contains(STwitch)) // crozza | twitch?
				{
					await command.RespondAsync("non hai il ruolo signor. twitch quindi non conti un cazzo");
					return;
				}

				SocketGuildUser cancro = (SocketGuildUser)command.Data.Options.ElementAt(0).Value;
				string name = cancro.Username;
				string discriminator = cancro.Discriminator;

				if (cancro.Id == command.User.Id)
				{
					await command.RespondAsync("porco dio sei disabile come francesco paolo farina");
					return;	
				}
				if (cancro.Id == AldioumaId || cancro.Id == PareId)
				{
					await command.RespondAsync($"ti ammazzo nel sonno");
					await command.Channel.SendFileAsync($"{ResourcePath}mihai ferro.png", "");
					return;
				}
				if (cancro.VoiceChannel is null)
				{
					await command.RespondAsync("dio negro non è in chat vocale");
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
			
			else if (command.CommandName == "rating")
			{
				SocketGuildUser user = (SocketGuildUser)command.Data.Options.ElementAt(0).Value;
				ISocketMessageChannel channel = command.Channel;

				await command.RespondAsync($"il cazzo e le palle di {user.Mention} ricevono il voto di...");

				switch (user.Id)
				{
					case AldioumaId: // aldiouma
						await channel.SendMessageAsync("10/10. sono bellissimo e ho tutte le fighe.");
						return;
					case VanniBotId: // vanni
						await channel.SendMessageAsync("-1/10. LUI È IL MIO ACERRIMO NEMICO. FA SCHIFO.");
						return;
					default:
						ulong vote = user.Id % 11;
						await channel.SendMessageAsync($"{vote}/10. {(vote == 0 ? "le palle sembrano uscite dal benin." : "")} {((vote == 10) ? "bellissime palle." : "")}");
						return;
				}
			}
		}
	}
}
