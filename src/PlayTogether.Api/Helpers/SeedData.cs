using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlayTogether.Infrastructure.Data;
using System;
using System.Linq;
using PlayTogether.Core.Dtos.Incoming.Generic;

namespace PlayTogether.Api.Helpers
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new AppDbContext(
                    serviceProvider.GetRequiredService<
                        DbContextOptions<AppDbContext>>())) {

                dbContext.Database.EnsureCreated();

                if (dbContext.AppUsers.Any()) {
                    var users = dbContext.AppUsers.ToList();
                    foreach (var user in users) {
                        var rates =  dbContext.Ratings.Where(x => x.ToUserId == user.Id).ToList();
                        var orders =  dbContext.Orders.Where(x => x.ToUserId == user.Id).ToList();
                        var orderOnTimes =  dbContext.Orders.Where(x => x.ToUserId == user.Id
                                                                            && x.Status == OrderStatusConstants.Finish).ToList();
                        double totalTime = 0;
                        foreach (var item in orders) {
                            totalTime += Infrastructure.Helpers.UtilsHelpers.GetTime(item.TimeStart, item.TimeFinish);
                        }
                        user.NumOfRate = rates.Count();
                        user.NumOfOrder = orders.Count();
                        user.TotalTimeOrder = Convert.ToInt32(Math.Round(totalTime / 3600));
                        user.NumOfFinishOnTime = orderOnTimes.Count();
                        dbContext.SaveChanges();
                    }
                }

                if (!dbContext.Games.Any()) {
                    dbContext.Games.AddRange(
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Arena of Valor",
                            DisplayName = "Liên Quân Mobile",
                            OtherName = "lq / aov / Lien Quan Mobile"
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "League Of Legends",
                            DisplayName = "Liên Minh Huyền Thoại",
                            OtherName = "Liên minh /  lol"
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Among Us",
                            DisplayName = "Among Us",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Age Of Empire",
                            DisplayName = "Đế Chế",
                            OtherName = "AOE"
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "DotA 2",
                            DisplayName = "DotA 2",
                            OtherName = "dota 2"
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Grand Theft Auto V",
                            DisplayName = "GTA 5",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Minecraft",
                            DisplayName = "Minecraft",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "World of Warcraft",
                            DisplayName = "Warcraft",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Supper Mario 64",
                            DisplayName = "Mario",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Angry Bird",
                            DisplayName = "Angry Bird",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Warface Mobile",
                            DisplayName = "Warface",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Call of Duty: Mobile",
                            DisplayName = "Call of Duty Mobile",
                            OtherName = ""
                        }
                        ,
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Battle Prime Online",
                            DisplayName = "Battle Prime",
                            OtherName = ""
                        }
                        ,
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Genshin Impact",
                            DisplayName = "Genshin Impact",
                            OtherName = ""
                        }
                        ,
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "PUBG Mobile",
                            DisplayName = "PUBG Mobile",
                            OtherName = ""
                        }
                        ,
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Free Fire",
                            DisplayName = "Free Fire",
                            OtherName = ""
                        }

                        ,
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Rules of Survival",
                            DisplayName = "Rules of Survival",
                            OtherName = ""
                        }
                        ,
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "FIFA Online 4",
                            DisplayName = "FIFA 4",
                            OtherName = "fifa"
                        }
                        ,
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "World of Tank",
                            DisplayName = "World of Tank",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Blade and Soul",
                            DisplayName = "Blade and Soul",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "MU Online",
                            DisplayName = "MU",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Haft Life",
                            DisplayName = "Haft Life",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Sea of Theives",
                            DisplayName = "Sea of Theives",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "NARAKA: BLADEPOINT",
                            DisplayName = "NARAKA",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Valorant",
                            DisplayName = "Valorant",
                            OtherName = ""
                        },
                        new Infrastructure.Entities.Game {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "PUBG PC",
                            DisplayName = "PUBG PC",
                            OtherName = ""
                        }
                    );

                }
                dbContext.SaveChanges();

                if (!dbContext.GameTypes.Any()) {
                    dbContext.GameTypes.AddRange(
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Shooter",
                            ShortName = "Shooter",
                            OtherName = "Bắn súng, FPS",
                            Description = "Game bắn súng là một trong những dạng game phổ biến nhất hiện nay. Những tựa game này đa phần là những dòng game bắn súng góc nhìn thứ nhất (FPS) bởi trải nghiệm cùng bề dày lịch sử của dòng game này vẫn chưa hề phai nhạt trong lòng game thủ. Một số game nổi tiếng của dòng game này đó là Counter Strike, Call of Duty."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Platformer",
                            ShortName = "Platformer",
                            OtherName = "Đi cảnh",
                            Description = "Game nền tảng mà chúng ta từng biết tới có lẽ là Mario Bros – một trong những thành công của hãng game Nintendo. Thể loại game này thường phổ biến trên các thế hệ game NES, SuperNES... và cách chơi khá đơn giản, vui nhộn nhưng đòi hỏi kĩ năng khéo léo, căn giờ, chỉnh góc thật chuẩn của người chơi."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Action",
                            ShortName = "Action",
                            OtherName = "Hành động",
                            Description = "Game hành động là một khái niệm game rộng lớn trong đó người chơi sẽ điều khiển nhân vật ở chính giữa màn hình và vượt qua các thử thách trong game đề ra. Để phân loại game hành động, người ta thường phân chia thành các thể loại sau."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Fighting",
                            ShortName = "Fighting",
                            OtherName = "Võ thuật, đối kháng",
                            Description = "Tựa game võ thuật, đối kháng là một trong những tựa game vui nhộn và thú vị bởi chúng ta có thể chơi tựa game này với hai người trên cùng một màn hình mà không ảnh hưởng tới trải nghiệm chơi. Một số tên tuổi tiêu biểu của dòng game này đó là Mortal Kombat (Rồng đen), Street Fighter hay Samurai Shodown."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Beat-em up",
                            ShortName = "Beat-em up",
                            OtherName = "Chặt chém, đấu võ",
                            Description = "Khi chơi những tựa game Beat – em up sẽ khiến người chơi cảm thấy đã tay vì được 'xả chiêu' liên tục vào đối thủ với những đòn combo hàng trăm hit. Tựa game này có lẽ thích hợp để chúng ta xả stress sau những giờ làm việc, học tập căng thẳng và có thể dồn nỗi tức giận lên các đối thủ trong game. Một số tựa game tiêu biểu trong thể loại này đó là God of War, Devil May Cry."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Steath",
                            ShortName = "Steath",
                            OtherName = "Hành động lén lút",
                            Description = "Tựa game hành động lén lút có lẽ là một trong những dòng game khá khó nhằn bởi chúng ta chỉ cần đi sai một bước và bị địch phát hiện thì bao nhiêu công sức sẽ 'đổ sông đổ bể'. Để vượt qua tựa game này thì người chơi phải có sự quan sát, tư duy chiến thuật và quan trọng nhất đó là căn thời gian, kết liễu kẻ địch thật nhanh và chuẩn để không bị phát hiện. Trong dòng game này thì một số cái tên như Metal Gear Solid, Hitman, Assasin's Creed là những tựa game tiêu biểu không thể bỏ qua nếu bạn là fan hâm mộ của dòng game này."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Survival",
                            ShortName = "Survival",
                            OtherName = "Sinh tồn",
                            Description = "Tham gia vào thế giới game sinh tồn, người chơi phải liên tục di chuyển, tìm kiếm các vật phẩm thiết yếu cho mục đích sinh tồn và mở khóa các kĩ năng chế tạo, xây dựng. Một số tựa game sinh tồn nổi bật đó là Stranded Deep, Ark Survival Evolved."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Rhythm",
                            ShortName = "Rhythm",
                            OtherName = "Giai điệu",
                            Description = "Được xếp vào danh sách game hành động nhưng những tựa game giai điệu không mang tính bạo lực nhiều mà mang lại những trải nghiệm giai điệu, âm nhạc cho người chơi. Những tựa game nổi tiếng của thể loại này đó là Guitar Hero, Dance Dance Revolution."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Action-Adventure",
                            ShortName = "Action-Adventure",
                            OtherName = "Game hành động phiêu lưu",
                            Description = "Game hành động phiêu lưu là sự kết hợp giữa yếu tố hành động, phiêu lưu giúp cho người cho có thể khám phá những bí mật trong game, thu thập các vật phẩm để sống sót hay tăng khả năng cho nhân vật chính."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Survival horror",
                            ShortName = "Survival horror",
                            OtherName = "Kinh dị - sinh tồn",
                            Description = "Tựa game này đưa người chơi vào những thử thách sinh tồn thực sự nhưng đôi lúc những tình huống kinh dị, ma quái sẽ khiến người yếu bóng vía có một phen 'khóc thét'. Những tựa game thuộc thể loại có thể kể đến như Redsident Evil, Amnesia: The Dark Descent..."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Metroidvania",
                            ShortName = "Metroidvania",
                            OtherName = "",
                            Description = "Đây là dòng game khá 'lạ tai' bởi chúng được khai tác từ hai tên tựa game nổi tiếng đó là Metroid và Castlevania của hai hàng Nintendo và Konami. Hiểu sơ sơ thì metroidvania là những tựa game có nền đồ họa 2D cùng với bản đồ rộng lớn, nhân vật phong phú và những tựa game này ngày nay đa số là những tựa game indie. Ví dụ: Ori and the Blind Forest, Iconoclasts..."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Adventure",
                            ShortName = "Adventure",
                            OtherName = "Game phiêu lưu",
                            Description = "Game phiêu lưu là những tựa game đưa người chơi vào khám phá cốt truyện, thế giới do nhà phát triển tạo ra, khiến người chơi được đắm chìm trong những cuộc phiêu lưu kì thú trong thế giới game."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Text game",
                            ShortName = "Text game",
                            OtherName = "Game tương tác",
                            Description = "Thể loại game này khá đơn giản và được các nhà lập trình game hoàn thiện trong một khoảng thời gian ngắn. Những tựa game này chủ yếu dùng các lệnh cơ bản để tương tác như chiến đấu (Fight), nhặt vũ khí (Grab Weapon)... Text game phát triển ở thế kỉ 20 khi nền công nghệ game còn non trẻ và máy tính chỉ có thể thực hiện những lệnh cơ bản mà chưa có giao diện đồ họa (Graphic). Một số tựa game tiêu biểu như: Planetfall, Spider and Web."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Graphic adventures",
                            ShortName = "Graphic adventures",
                            OtherName = "Trò chơi phiêu lưu đồ họa",
                            Description = "Ở thể loại này thì dòng game phiêu lưu nâng thêm một tầng cao mới đó là được trang bị giao diện đồ họa và người chơi có thể tương tác vào các vật phẩm trong game với cú click chuột. Ví dụ: Snatcher (Konami), Spycraft: The Great Game (Activision), Shadow of Memories (Konami)."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Visual Novel",
                            ShortName = "Visual Novel",
                            OtherName = "Tiểu thuyết hình ảnh",
                            Description = "Dòng game này được biết đến với cách chơi đơn giản nhưng vô cùng ngộ nghĩnh: nhập vai, dùng lời thoại để điều khiển nhân vật với hình ảnh tĩnh. Ngày nay, những tựa game tiểu thuyết hình ảnh thường là những tựa game khai thác từ anime giúp cho chúng ta như được xem một cuốn truyện manga đầy sinh động trên hình ảnh của anime vậy. Ví dụ: Phoenix Wright, Danganronpa."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Role-playing game",
                            ShortName = "RPG",
                            OtherName = "Nhập vai",
                            Description = "Game nhập vai là một trong những thể loại game 'cày cuốc' giết thời gian của người chơi nhiều nhất. Để sở hữu những thông số khủng trong game thì người chơi phải bỏ thời gian đi tăng Level khi đánh quái, boss hay tìm kiếm những vật phẩm hiếm bằng cách vượt qua những nhiệm vụ, trùm trong game."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Massively Multiplayer Online Role-Playing Game",
                            ShortName = "MMORPG",
                            OtherName = "Nhập vai trực tuyến RPG",
                            Description = "Những thể loại game nhập vai trực tuyến mang yếu tố của game nhập vai (RPG) nhưng người chơi có thể so tài với các người chơi khác qua Internet. Đây là một trong những thể loại game gây nghiện và tiêu tốn khá nhiều thời gian hoặc tiền bạc của game thủ bởi nhà phát hành game thường tổ chức các sự kiện với các hình thức nạp thẻ mua vật phẩm hay bán các vật phẩm hiếm quy đổi bằng tiền mặt. Vào những năm đầu thế kỉ 21 thì những tựa game MMORPG khá phổ biến ở Việt Nam và nhiều game thủ sẵn sàng chi tiền triệu vào những nhân vật ảo trong game Thiên Long Bát Bộ, MU Online, Võ Lâm Truyền Kỳ."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Tactical RPG",
                            ShortName = "Tactical RPG",
                            OtherName = "Nhập vai chiến lược",
                            Description = "Thể loại game này yêu cầu người chơi đưa ra các chiến lược khả thi để hoàn thành các thử thách trong game. Dòng game này vẫn mang bản chất cơ bản của RPG bởi lối chơi nhập vai nhân vật và tăng kĩ năng, vật phẩm trong game nhờ tiêu diệt quái, lính hay boss. Ví dụ: Fallout Tactics: Brotherhood of Steel, Warhamer 40,000."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Sandbox RPG",
                            ShortName = "Sandbox RPG",
                            OtherName = "Nhập vai thế giới mở",
                            Description = "Gọi chung là game 'sandbox' cũng được, những tựa game này đưa người chơi vào những cuộc phiêu lưu không luật lệ. Người chơi có thể tự do khám phá thế giới theo cách riêng của bạn mà không cần phải đi theo cốt truyện (Story). Một trong những 'tượng đài' của làng game Sandbox là Minecraft với hàng triệu game thủ trên thế giới chơi hàng ngày."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "First person party based RPG",
                            ShortName = "First person party based RPG",
                            OtherName = "Game nhập vai nhóm theo lượt",
                            Description = "Những tựa game này có lối chơi khá đơn giản. Người chơi sẽ nhập vai vào một nhóm nhân vật với vài thành viên và di chuyển, tấn công theo lượt. Dòng game nổi tiếng Might and Magic là tựa game thuộc thể loại này."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Simulation",
                            ShortName = "Simulation",
                            OtherName = "Game mô phỏng",
                            Description = "Game mô phỏng khá là hay và gây 'nghiện' bởi tính thực tế và đáng yêu. Đôi khi, bạn muốn làm một điều gì đó thực tế nhưng tuổi tác cũng như khả năng của bạn không thể thực hiện được và game mô phỏng ra đời giúp cho người chơi có thể thử làm một điều gì đó thú vị như làm một người trưởng thành, lái máy bay, ô tô hoặc quản lý cả một thành phố."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Construction and management simulation",
                            ShortName = "Construction and management simulation",
                            OtherName = "Mô phỏng xây dựng và quản lý",
                            Description = "Tựa game mô phỏng xây dựng và quản lý đưa người chơi vào vai những nhà quản lý, chính quyền của một đơn vị, thành phố, đất nước và sử dụng hợp lý nguồn tài nguyên để phát triển thành phố, cơ sở vật chất. Những tựa game tiêu biểu của thể loại này đó là SimCit, Roller Coaster Tycoon."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Life simulation",
                            ShortName = "Life simulation",
                            OtherName = "Mô phỏng đời sống sinh hoạt",
                            Description = "Game mô phỏng đời sống đưa bạn nhập vai vào một nhân vật và sống trong thế giới với nhiều nhân vật khác nhau. Bạn có thể kết bạn bất cứ ai, nộp đơn ứng tuyển bất kì vị trí công việc thăng tiến nào và lựa chọn bạn đời. Một trong những tượng đài của dòng game này đó chính là The Sims."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Vehicle simulation",
                            ShortName = "Vehicle simulation",
                            OtherName = "Mô phỏng phương tiện",
                            Description = "Bạn đã bao giờ ao ước được ngồi trên những chiếc xe container vô cùng hoành tráng hay lái những chiếc phi cơ siêu hạng chưa? Để thỏa mãn những cảm giác thú vị này thì  bạn hãy tìm đến những tựa game mô phỏng phương tiện như Truck Simulation hay Flight Simulation để giải trí và đem lại cảm giác cầm lái phương tiện mà bạn yêu thích."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Strategy",
                            ShortName = "Strategy",
                            OtherName = "Game chiến thuật",
                            Description = "Game chiến thuật là một trong những thế loại game khá phổ biến hiện nay. Chơi game chiến thuật yêu cầu người chơi phải có phán đoán, khả năng quan sát và tính toán hợp lý nhất. Những tựa game chiến thuật giúp người chơi khả năng tư duy sắc bén và nhanh nhạy."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Explore, expand, exploit, exterminate",
                            ShortName = "Explore",
                            OtherName = "Chiến thuật 4X",
                            Description = ""
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Artillery",
                            ShortName = "Artillery",
                            OtherName = "Chiến thuật pháo binh",
                            Description = "Dòng game này thường lấy đề tài các loại pháo binh, vũ khí chiến lược như xe tăng, tàu vũ trụ hoặc thậm chí là các loại vũ khí hạng nặng như bazoka. Tiêu biểu của dòng game này đó là tựa game bắn tăng huyền thoại (Battle City) trên hệ máy NES hay trò bắn sâu nổi tiếng một thời (Worm) trên máy tính."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Real-time strategy",
                            ShortName = "RTS",
                            OtherName = "Chiến thuật thời gian thực",
                            Description = "Game chiến thuật thời gian thực khai thác các yếu tố như khai tác tài nguyên, xây dựng, phát triển, chiến tranh và người chơi phải quản lý, sử dụng và lập các kế hoạch để phát triển, chinh phục đối thủ trong thời gian ngắn nhất. Ví dụ: Age of Empires, Red Alert."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Real-time tactics",
                            ShortName = "RTT",
                            OtherName = "Chiến lược thời gian thực",
                            Description = "Thể loại RTT là một nhánh nhỏ trong dòng game RTS. Game RTT tập trung vào phát triển các chiến lược cụ thể để giành chiến thắng thay vì phải thu thập tài nguyên, xây nhà cửa, huấn luyện quân đội. Ví dụ: Modem War (Ozark), Close Combat: Invasion Normandy (Atomic)."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Multiplayer online battle arena",
                            ShortName = "MOBA",
                            OtherName = "Đấu trường trực tuyến đa người chơi",
                            Description = "Thể loại game này về cơ bản là việc 'nhồi nhét' từ 2 – 10 người chơi vào một 'đấu trường' ảo, cho họ chiến đấu và giành chiến thắng. Tuy nhiên, tựa game MOBA yêu cầu tinh thần đồng đội cao, sự phối hợp giữa các đơn vị tướng do người chơi điều khiển, tận dụng các yếu tố như điểm hạ gục (Kill), hỗ trợ (Assist) và nâng cấp trang bị sao cho phù hợp với ván đấu. Cho đến nay thì tựa game khá lâu đời và được nhiều game thủ yêu thích đó là League of Legends và DOTA 2."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Turn base strategy",
                            ShortName = "TBS",
                            OtherName = "Chiến thuật theo lượt",
                            Description = "Game chiến thuật theo lượt xuất hiện từ rất sớm, xuất hiện trên các tựa game trên dòng máy console như SNES, PS1, XBOX bởi khả năng di chuyển theo lượt và không cần độ chính xác, di chuyển nhiều như game chiến thuật thời gian thực (RTS). Nếu bạn từng sử dụng hệ máy như PS1, PS2 thì chắc chắn bạn đã nghe qua 'siêu phầm' Final Fantasy 7, 8, 10 huyền thoại từng làm 'say đắm' biết bao thế hệ game thủ."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Turn base tactics",
                            ShortName = "TBT",
                            OtherName = "Chiến lược theo lượt",
                            Description = "Là một nhánh nhỏ của dòng game TBS, game chiến thuật theo lượt tập trung vào các chiến lược là chủ yếu, không yêu cầu người chơi phải cày cuốc, thu thập vật phẩm và trước khi 'xông pha' chiến đấu, người chơi cần trang bị những món vũ khí phù hợp cho các lớp (class) nhân vật khác nhau để giành chiến thắng ván chơi. Ví dụ: XCOM: Enemy Unknown, Chrono Trigger, Final Fantasy Tactics."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Tower defense",
                            ShortName = "Tower defense",
                            OtherName = "Thủ thành",
                            Description = "Game chiến thuật thủ thành có lối chơi vô cùng đơn giản nhưng yêu cầu người chơi phải biết tối ưu tài nguyên và nâng cấp các trang bị cho các đợt tấn công một cách hợp lý để giành chiến thắng. Dòng game thủ thành trên máy tính và cả điện thoại được rất nhiều người yêu thích đó là Plant vs Zombie, Fieldrunner."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Wargame",
                            ShortName = "Wargame",
                            OtherName = "Chiến lược chiến tranh",
                            Description = "Lấy đề tài chiến tranh thực tế và người chơi phải đưa ra các chiến lược cụ thể trong cuộc chiến để giành thắng lợi như chiếm các căn cứ trọng điểm, tổ chức kế hoạch đánh du kích... Ví dụ như trò Real War, Men of War."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Grand strategy wargame",
                            ShortName = "Grand strategy wargame",
                            OtherName = "Chiến thuật chiến tranh đa quốc gia",
                            Description = "Cũng khai thác đề tài chiến tranh nhưng bối cảnh game sẽ lấy đề tài về quân đội các nước cùng với số lượng đông đảo các đơn vị lính trên chiến trường, đôi khi lên tới hàng chục nghìn binh lính giúp cho người chơi trải nghiệm những trận chiến có một không hai như trong lịch sử thế giới. Cái tên tiêu biểu của dòng game này đó chính là Total War."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Sport Game",
                            ShortName = "Sport",
                            OtherName = "Game thể thao",
                            Description = "Game thể thao thường là những tựa game khai thác những đề tài thể thao như bóng đá, đua xe, võ thuật... là là một trong những tựa game lành mạnh mà chúng ta nên giải trí thay vì chơi những tựa game gây nghiện, ám ảnh như game kinh dị, bắn súng, thế giới mở."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Racing",
                            ShortName = "Racing",
                            OtherName = "Đua xe",
                            Description = "Những tựa game đua xe đưa người chơi nhập vai vào người cầm lái những cỗ xe nổi tiếng thế giới như xe đua công thức 1, 'siêu bò' Lamborghini.... và trải nghiệm cảm giác làm tay đua thứ thiệt trong game. Để trải nghiệm cảm giác chân thực thì người chơi có thể sắm những bộ vô lăng để chơi game 'đã' hơn. Những tựa game tiêu biểu của dòng game đua xe đó là F1, Forza."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Team sports",
                            ShortName = "Team sports",
                            OtherName = "Phối hợp đồng đội",
                            Description = "Tham gia những tựa game phối hợp đồng đội, người chơi có cơ hội được đắm mình trong những trận cầu siêu kinh điển cùng dàn cầu thủ ngôi sao hay thưởng thức giải bóng rổ nhà nghề Mỹ NBA ngay trên màn hình của bạn. Ví dụ: FIFA, PES, Madden NFL, NBA2K."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Competitive",
                            ShortName = "Competitive",
                            OtherName = "Thi đấu",
                            Description = "Thể loại game này còn được gọi là môn thể thao điện tử (eSport) và những tựa gaem của dòng này có thể là game bắn súng FPS hay game phối hợp đồng đội. Đây là những tựa game được lựa chọn và tổ chức thành các giải đấu chuyên nghiệp quy tụ các game thủ trên toàn thế giới với giải thưởng hàng nghìn đô la. Ví dụ: Overwatch, Team Fortress."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Sports-based fighting",
                            ShortName = "Sports-based fighting",
                            OtherName = "Đấu võ",
                            Description = "Những tựa game này dựa trên các môn thể thao truyền thống như võ thuật, boxing và người chơi được nhập vai trong các môn võ vô cùng thú vị. Ví dụ: Fight Night Round, WWE 2K."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Puzzle",
                            ShortName = "Puzzle",
                            OtherName = "Game giải đố",
                            Description = "Game giải đố là một trong những thể loại game giết thời gian nhưng khá gây nghiện bởi luật chơi đơn giản nhưng lại khiến người chơi gian nan tìm hiểu những mấu chốt để chiến thắng. Đây còn là thể loại game rèn luyện trí não cực tốt mà bạn nên chơi thử."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Logic",
                            ShortName = "Logic",
                            OtherName = "Tư duy",
                            Description = "Game tư duy đơn giản là đưa người chơi vào những tình huống đòi hỏi sự tư duy như xếp một khối gạch sao cho khít hay tìm lối thoát trong mê cung. Ví dụ: Tetris, Bejeweled."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Trivia",
                            ShortName = "Trivia",
                            OtherName = "Câu đố",
                            Description = "Thể loại game này yêu cầu người chơi phải giải đố trước thời gian kết thúc. Mỗi câu đố có thể tồn tại dưới dạng trắc nghiệm với đáp án A, B, C, D trước khi hết giờ. Nghe có vẻ giống như chương trình Ai là triệu phú trên VTV3. Đúng vậy, gameshow Ai là triệu phú từng được làm thành game trivia trên máy tính cùng tên. Ngoài ra, chúng ta còn chứng kiến tựa game gây sốt với thể loại này trên mạng xã hội đó là Confetti với giải thưởng hàng nghìn đô la."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Casual",
                            ShortName = "Casual",
                            OtherName = "Game ngắn, đơn giản",
                            Description = "Đây là thể loại game khá đông, nhất là từ khi nền tảng Android và iOS phát triển thì lượng game đổ bộ trên nền tảng này khá nhiều. Game casual được hiểu là game ngắn, cách chơi đơn giản. Một số thể loại game nổi tiếng của dòng game này đó là Candy Crush Saga, Flappy Bird."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Card game/ Board game",
                            ShortName = "Card game/ Board game",
                            OtherName = "Game thẻ bài",
                            Description = "Game này đưa người chơi những mẫu thẻ bài và dùng chúng để chiến đấu với người chơi khác dựa vào kĩ năng, chỉ số của thẻ bài so với đối thủ. Trước khi Autochess (DOTA 2) và Team Fight Tactics (League of Legends) có mặt thì tựa game Yugioh ăn theo bộ manga nổi tiếng cùng tên là một trong những tựa game thẻ bài ăn khách nhất."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Game based learing",
                            ShortName = "Game based learing",
                            OtherName = "Game giáo dục",
                            Description = "Ý tưởng học mà chơi, chơi mà học khá là hay khi được các nhà trường áp dụng các hình thức chơi game để giáo dục cho các học sinh của mình. Những tựa game giáo dục thường mang hình ảnh minh họa sinh động, ngộ nghĩnh thường nhắm vào đối tượng học sinh cấp 1, cấp 2 vời những bài học về toán, văn học, lịch sử...Trong một số trường học thì học sinh được học cách gõ phím với mười đầu ngón tay qua tựa game Mario Teaches Typing."
                        },
                        new Infrastructure.Entities.GameType {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            Name = "Excergame",
                            ShortName = "Excergame",
                            OtherName = "Game vận động",
                            Description = "Thay vì ngồi lì một chỗ thì những tựa game này yêu cầu người chơi phải vận động theo các động tác của game. Một trong những dòng máy chơi game hỗ trợ các tựa game này đó là Wii Sport như Let's Golf và gần đây nhất là phiên bản cầm tay Nintendo Switch với những tựa game yêu cầu người chơi phải vận động như Mario tennis aces."
                        }
                    );
                }
                dbContext.SaveChanges();

                if (!dbContext.Ranks.Any()) {
                    var aov = dbContext.Games.FirstOrDefault(x => x.Name == "Arena of Valor");
                    var lol = dbContext.Games.FirstOrDefault(x => x.Name == "League Of Legends");
                    dbContext.AddRange(
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 1,
                            Name = "Đồng",
                            GameId = aov.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 2,
                            Name = "Bạc",
                            GameId = aov.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 3,
                            Name = "Vàng",
                            GameId = aov.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 4,
                            Name = "Bạch Kim",
                            GameId = aov.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 5,
                            Name = "Kim Cương",
                            GameId = aov.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 6,
                            Name = "Tinh Anh",
                            GameId = aov.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 7,
                            Name = "Cao Thủ",
                            GameId = aov.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 8,
                            Name = "Chiến Tướng",
                            GameId = aov.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 9,
                            Name = "Thách Đấu",
                            GameId = aov.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 1,
                            Name = "Sắt",
                            GameId = lol.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 2,
                            Name = "Đồng",
                            GameId = lol.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 3,
                            Name = "Bạc",
                            GameId = lol.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 4,
                            Name = "Vàng",
                            GameId = lol.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 5,
                            Name = "Bạch Kim",
                            GameId = lol.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 6,
                            Name = "Kim Cương",
                            GameId = lol.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 7,
                            Name = "Cao Thủ",
                            GameId = lol.Id
                        },
                        new Infrastructure.Entities.Rank {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            NO = 8,
                            Name = "Thách Đấu",
                            GameId = lol.Id
                        }
                    );
                }
                dbContext.SaveChanges();

                if (!dbContext.TypeOfGames.Any()) {
                    var aov = dbContext.Games.FirstOrDefault(x => x.Name == "Arena of Valor");
                    var lol = dbContext.Games.FirstOrDefault(x => x.Name == "League Of Legends");
                    var among = dbContext.Games.FirstOrDefault(x => x.Name == "Among Us");

                    var survival = dbContext.GameTypes.FirstOrDefault(x => x.Name == "Survival");
                    var moba = dbContext.GameTypes.FirstOrDefault(x => x.ShortName == "MOBA");

                    dbContext.TypeOfGames.AddRange(
                        new Infrastructure.Entities.TypeOfGame {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            GameTypeId = moba.Id,
                            GameId = lol.Id
                        },
                        new Infrastructure.Entities.TypeOfGame {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            GameTypeId = moba.Id,
                            GameId = aov.Id
                        },
                        new Infrastructure.Entities.TypeOfGame {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null,
                            GameTypeId = survival.Id,
                            GameId = among.Id
                        }
                    );
                }
                dbContext.SaveChanges();

                // if (!dbContext.BehaviorPoints.Any()){
                //     var users = dbContext.AppUsers.ToList();

                //     foreach (var user in users)
                //     {
                //         dbContext.BehaviorPoints.Add(
                //             new Infrastructure.Entities.BehaviorPoint{
                //                 Id = Guid.NewGuid().ToString(),
                //                 CreatedDate = DateTime.UtcNow.AddHours(7),
                //                 UserId = user.Id,
                //                 Point = 100,
                //                 SatisfiedPoint = 100
                //             }
                //         );
                //         dbContext.SaveChanges();
                //     }
                // }
            }
        }
    }
}