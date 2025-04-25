using System;
using cAlgo.API;
using System.Linq;
using System.Drawing;
using Color = cAlgo.API.Color;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace Market_Sessions_Advanced
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class Market_Sessions_Advanced : Indicator
    {
        public enum SessionColor
        {
            Red, Green, Blue,
            Yellow, Orange, Purple,
            Pink, Cyan, Brown, DarkRed,
            White, Black, Gray, Indigo,
            LightBlue, LightGreen, LightGray,
            LightPink, LightYellow,
            DarkBlue, DarkGreen,DarkYellow,
            DarkGray, DarkOrange, DarkPink,


        }
        public enum ObjectToDraw
        {
            Rectangle, Lines
        }
        public enum Lines
        {
            Solid, Dots, DotsRare, DotsVeryRare, Dashes, DashDot
        }
        public enum Position
        {
            Top, Bottom
        }

        [Parameter("CopyRight © 2025", DefaultValue = "Arnyfesto1.  All Rights Reserved", Group = "About")]
        public string CopyRight { get; set; }
        [Parameter("Version", DefaultValue = "2.1.7", Group = "About")]
        public string Version { get; set; }
        [Parameter("Support", DefaultValue = "+255 750 240 859 whatsApp & Telegram", Group = "About")]
        public string Author { get; set; }


        [Parameter("Enable DST", DefaultValue = true, Group = "Setting")]
        public bool EnableDST { get; set; }

        [Parameter("Show Asian", DefaultValue = true, Group = "Asian")]
            public bool ShowAsian { get; set; }
        [Parameter("Asian Style", DefaultValue = ObjectToDraw.Lines, Group = "Asian")]
            public ObjectToDraw AsianStyle { get; set; }
        [Parameter("Asian Line",DefaultValue = Lines.Solid, Group = "Asian")]
            public Lines AsianLineStyle { get; set; }
        [Parameter("Asian Color", DefaultValue = SessionColor.Red, Group = "Asian")]
        public SessionColor AsianColor { get; set; }


        [Parameter("Show London", DefaultValue = true, Group = "London")]
            public bool ShowLondon { get; set; }
        [Parameter("London Style", DefaultValue = ObjectToDraw.Lines, Group = "London")]
            public ObjectToDraw LondonStyle { get; set; }
        [Parameter("London Line", DefaultValue = Lines.Solid, Group = "London")]
            public Lines LondonLineStyle { get; set; }
        [Parameter("London Color", DefaultValue = SessionColor.Blue, Group = "London")]
            public SessionColor LondonColor { get; set; }

        [Parameter("Show NewYork", DefaultValue = true, Group = "New York")]
            public bool ShowNewYork { get; set; }
        [Parameter("NewYork Style", DefaultValue = ObjectToDraw.Lines, Group = "New York")]
            public ObjectToDraw NewYorkStyle { get; set; }
        [Parameter("NewYork Line", DefaultValue = Lines.Solid, Group = "New York")]
            public Lines NewYorkLineStyle { get; set; }

        [Parameter("NewYork Color", DefaultValue = SessionColor.Green, Group = "New York")]
        public SessionColor NewYorkColor { get; set; }


        [Parameter("Show Silver Bullet", DefaultValue = false, Group = "Silver Bullet")]
            public bool ShowSilverBullet { get; set; }
        [Parameter("Silver Bullet Style", DefaultValue = ObjectToDraw.Rectangle, Group = "Silver Bullet")]
            public ObjectToDraw SilverBulletStyle { get; set; }
        [Parameter("Silver Bullet Line", DefaultValue = Lines.Solid, Group = "Silver Bullet")]
            public Lines SilverBulletLine { get; set; }
        [Parameter("Silver Bullet Color", DefaultValue = SessionColor.Gray, Group = "Silver Bullet")]
            public SessionColor SilverBulletColor { get; set; }



        [Parameter("Show Killzones", DefaultValue = false, Group = "ICT Killzones")]
            public bool ShowKillzones {get; set;}
        [Parameter("Killzone Style", DefaultValue = ObjectToDraw.Rectangle, Group = "ICT Killzones")]
            public ObjectToDraw KillzoneStyle {get; set;}
        [Parameter("Killzone Position", DefaultValue = Position.Bottom, Group = "ICT Killzones")]
            public Position KillzonePosition {get; set;}
        [Parameter("Asian Color", DefaultValue = SessionColor.DarkRed, Group = "ICT Killzones")]
            public SessionColor AsianKillzoneColor {get; set;}
        [Parameter("London Color", DefaultValue = SessionColor.Blue, Group = "ICT Killzones")]
            public SessionColor LondonKillzoneColor {get; set;}
        [Parameter("NewYork Color", DefaultValue = SessionColor.Green, Group = "ICT Killzones")]
        public SessionColor NewYorkKillzoneColor {get; set;}



        [Parameter("Show Daily Open", DefaultValue = false, Group = "Opening Ranges")]
            public bool ShowDailyOpen {get; set;}
        [Parameter("Daily LineStyle", DefaultValue = Lines.Dots, Group = "Opening Ranges")]
            public Lines DailyOpenLine {get; set;}
        [Parameter("Daily Open Color", DefaultValue = SessionColor.Purple, Group = "Opening Ranges")]
        public SessionColor DailyOpenColor {get; set;}

        [Parameter("Show Weekly Open", DefaultValue = false, Group = "Opening Ranges")]
            public bool ShowWeeklyOpen {get; set;}
        [Parameter("Weekly LineStyle", DefaultValue = Lines.Dots, Group = "Opening Ranges")]
            public Lines WeeklyOpenLine {get; set;}
        [Parameter("Weekly Color", DefaultValue = SessionColor.LightBlue, Group = "Opening Ranges")]
            public SessionColor WeeklyOpenColor {get; set;}
        [Parameter("Show Monthly Open", DefaultValue = false, Group = "Opening Ranges")]
            public bool ShowMonthlyOpen {get; set;}
        [Parameter("Monthly LineStyle", DefaultValue = Lines.Dots, Group = "Opening Ranges")]
            public Lines MonthlyOpenLine {get; set;}
        [Parameter("Monthly Color", DefaultValue = SessionColor.LightYellow, Group = "Opening Ranges")]
        public SessionColor MonthlyOpenColor {get; set; }


        private Dictionary<string, (TimeSpan Start, TimeSpan End, bool Show, ObjectToDraw Style,  Lines lineStyle, SessionColor colorName)> Sessions;
        private Dictionary<string, (TimeSpan start, TimeSpan end, bool show, ObjectToDraw Style, SessionColor colorName)> ICTKillzones;        

        protected override void Initialize()
        {
            Sessions = new Dictionary<string, (TimeSpan, TimeSpan, bool, ObjectToDraw, Lines, SessionColor )>
            {
                { "Asian", (new TimeSpan(01, 0, 0), new TimeSpan(10, 0, 0), ShowAsian, AsianStyle, AsianLineStyle, AsianColor) },
                { "London", (new TimeSpan(08, 0, 0), new TimeSpan(17, 0, 0), ShowLondon, LondonStyle, LondonLineStyle, LondonColor) },
                { "New York", (new TimeSpan(13, 0, 0), new TimeSpan(22, 0, 0), ShowNewYork, NewYorkStyle, NewYorkLineStyle, NewYorkColor) }
            };
            ICTKillzones = new Dictionary<string, (TimeSpan, TimeSpan, bool, ObjectToDraw, SessionColor)>
            {
                { "AsianKillzone", (new TimeSpan(00, 0, 0), new TimeSpan(03, 0, 0), ShowKillzones, KillzoneStyle, AsianKillzoneColor) },
                { "LondonOpenKillzone", (new TimeSpan(07, 0, 0), new TimeSpan(10, 0, 0), ShowKillzones, KillzoneStyle, LondonKillzoneColor) },
                { "LondonCloseKillzone", (new TimeSpan(16, 0, 0), new TimeSpan(18, 00, 0), ShowKillzones, KillzoneStyle, LondonKillzoneColor) },
                { "NewYorkOpenKillzone", (new TimeSpan(12, 0, 0), new TimeSpan(15, 0, 0), ShowKillzones, KillzoneStyle, NewYorkKillzoneColor) },
                { "NewYorkCloseKillzone", (new TimeSpan(19, 0, 0), new TimeSpan(21, 0, 0), ShowKillzones, KillzoneStyle, NewYorkKillzoneColor) }
            };

            Chart.DrawStaticText("SessionsInfo", "Market Sessions", 
                VerticalAlignment.Top, HorizontalAlignment.Center, Color.White);
        }

        public override void Calculate(int index)
        {
            DateTime today = Bars.OpenTimes[index].Date;
            DateTime yesterday = today.AddDays(-1);
            DateTime beforeYesterday = yesterday.AddDays(-2);

            foreach (var session in Sessions)
            {
                if (!session.Value.Show) continue;
                    DrawSessionLines(session.Key, today, (session.Value.Start, session.Value.End, session.Value.Show, session.Value.Style, session.Value.lineStyle, session.Value.colorName.ToString()), "Today");
                    DrawSessionLines(session.Key, yesterday, (session.Value.Start, session.Value.End, session.Value.Show, session.Value.Style, session.Value.lineStyle, session.Value.colorName.ToString()), "Yesterday");
                    DrawSessionLines(session.Key, beforeYesterday, (session.Value.Start, session.Value.End, session.Value.Show, session.Value.Style, session.Value.lineStyle, session.Value.colorName.ToString()), "2 Days Ago");
            }
            
            // Draw ICT Killzones
            TimeSpan dstOffset = EnableDST ? TimeSpan.FromHours(-1) : TimeSpan.Zero;
            foreach (var killzone in ICTKillzones)
            {
                if (killzone.Key == "AsianKillzone" && ShowKillzones ||
                    killzone.Key == "LondonOpenKillzone" && ShowKillzones ||
                    killzone.Key == "LondonCloseKillzone" && ShowKillzones ||
                    killzone.Key == "NewYorkOpenKillzone" && ShowKillzones  ||
                    killzone.Key == "NewYorkCloseKillzone" && ShowKillzones)
                {
                    DateTime killzoneStart = today + killzone.Value.start + dstOffset;
                    DateTime killzoneEnd = today + killzone.Value.end + dstOffset;
                    DrawKillzone(killzone.Key, killzoneStart, killzoneEnd);
                }
            }

            // Detect Silver Bullet
            if(ShowSilverBullet)
            {
                DateTime silverBulletStart = today.AddHours(15) + dstOffset;
                DateTime silverBulletEnd = today.AddHours(16) + dstOffset;

                int start = Bars.OpenTimes.GetIndexByTime(silverBulletStart);
                int end = Bars.OpenTimes.GetIndexByTime(silverBulletEnd);

                if (Server.Time < silverBulletStart)
                    return;

                DetectSilverBullet(start, end);
            }
                

            // Draw Daily, Weekly, and Monthly Open lines
            TimeSpan dstAdjustment = EnableDST ? TimeSpan.FromHours(-1) : TimeSpan.Zero;
            if (ShowDailyOpen) 
            {
                DateTime openingDay = yesterday.AddHours(23);
                DrawDailyOpen(openingDay + dstAdjustment);
            }
            // Draw Weekly Open line
            if (ShowWeeklyOpen) 
            {
                DateTime weeklyOpenDay = today.AddDays(-(int)today.DayOfWeek).AddHours(23);
                DrawWeeklyOpen(weeklyOpenDay + dstAdjustment);
            }
            // Draw Monthly Open line
            if (ShowMonthlyOpen) 
            {
                DateTime firstDayOfMonth = new(today.Year, today.Month, 1);
                DateTime monthlyOpenDay = firstDayOfMonth.AddHours(23);
                DrawMonthlyOpen(monthlyOpenDay + dstAdjustment);
            }
        }

        private void DrawSessionLines(string sessionName, DateTime sessionDate, 
                                      (TimeSpan Start, TimeSpan End, bool Show, ObjectToDraw Style, Lines LineStyle, string ColorName) sessionData, 
                                      string labelSuffix)
        {
            TimeSpan dstAdjustment = EnableDST ? TimeSpan.FromHours(-1) : TimeSpan.Zero;

            DateTime sessionStart = sessionDate + sessionData.Start + dstAdjustment;
            DateTime sessionEnd = sessionDate + sessionData.End + dstAdjustment;

            int startX = Bars.OpenTimes.GetIndexByTime(sessionStart);
            int endX = Bars.OpenTimes.GetIndexByTime(sessionEnd);

            if (startX < 0 || endX < 0 || startX >= endX) return;

            double sessionHigh = Bars.HighPrices.Skip(startX).Take(endX - startX + 1).Max();
            double sessionLow = Bars.LowPrices.Skip(startX).Take(endX - startX + 1).Min();

            Color sessionColor = GetColors((SessionColor)Enum.Parse(typeof(SessionColor), sessionData.ColorName));

            if(sessionData.Style == ObjectToDraw.Lines)
            {
                Chart.DrawTrendLine(
                    $"{sessionName}{labelSuffix}High", 
                    Bars.OpenTimes[startX], sessionHigh, Bars.OpenTimes[endX], sessionHigh, 
                    sessionColor, 2, GetLineStyle(sessionData.LineStyle)
                );
                Chart.DrawTrendLine(
                    $"{sessionName}{labelSuffix}Low", 
                    Bars.OpenTimes[startX], sessionLow, Bars.OpenTimes[endX], sessionLow, 
                    sessionColor, 2, GetLineStyle(sessionData.LineStyle)
                );
            }
            else if(sessionData.Style == ObjectToDraw.Rectangle)
            {
                Chart.DrawRectangle(
                    $"{sessionName}{labelSuffix}Range", 
                    Bars.OpenTimes[startX], sessionHigh, Bars.OpenTimes[endX], sessionLow, 
                    sessionColor, 1, GetLineStyle(sessionData.LineStyle)
                );
            }
            Chart.DrawText(
                $"{sessionName}{labelSuffix}Label", 
                $"{sessionName}", startX, 
                sessionHigh, sessionColor
            );
        }
        
        private void DrawKillzone(string killzoneName, DateTime killzoneStart, DateTime killzoneEnd)
        {
            int startX = Bars.OpenTimes.GetIndexByTime(killzoneStart);
            int endX = Bars.OpenTimes.GetIndexByTime(killzoneEnd);
            if (startX < 0 || endX < 0 || startX >= endX) return;

            Color killzoneColor = Color.FromName((killzoneName switch
            {
                "AsianKillzone" => AsianKillzoneColor,
                "LondonOpenKillzone" => LondonKillzoneColor,
                "LondonCloseKillzone" => LondonKillzoneColor,
                "NewYorkOpenKillzone" => NewYorkKillzoneColor,
                "NewYorkCloseKillzone" => NewYorkKillzoneColor,
                _ => SessionColor.White
            }).ToString());

            double adjacent = Symbol.PipSize * 10; // Adjust this value as needed
            double lowestPrice = Chart.BottomY + adjacent; // Get the lowest price on the chart
            double highestPrice = Chart.TopY - adjacent; // Get the highest price on the chart
            double position = KillzonePosition == Position.Top ? highestPrice : lowestPrice;

            if(KillzoneStyle == ObjectToDraw.Rectangle)
            {
                Chart.DrawRectangle(
                    $"{killzoneName}Rectangle", startX, position, 
                    endX, position,
                    killzoneColor, 5, LineStyle.Solid
                ).IsFilled = true;
            }
            else if(KillzoneStyle == ObjectToDraw.Lines)
            {
               Chart.DrawTrendLine($"{killzoneName}Line", startX, position , 
                    endX, position , 
                    killzoneColor, 10, LineStyle.Dots
                );
            }
        }
        private void DetectSilverBullet(int startX, int endX)
        {
            if(!ShowSilverBullet) return;

            // Get high and low during this period
            double high = Bars.HighPrices.Skip(startX).Take(endX - startX + 1).Max();
            double low = Bars.LowPrices.Skip(startX).Take(endX - startX + 1).Min();

            // Draw Silver Bullet zone
            if (SilverBulletStyle == ObjectToDraw.Rectangle)
            {
                Chart.DrawRectangle(
                    "SilverBullet", startX, high, endX, low,
                    GetColors(SilverBulletColor), 1, GetLineStyle(SilverBulletLine)
                );
            }
            else if (SilverBulletStyle == ObjectToDraw.Lines)
            {
                Chart.DrawTrendLine(
                    "SilverBulletUp", startX, high, Math.Min(endX, Bars.Count - 1), high,
                    GetColors(SilverBulletColor), 1, GetLineStyle(SilverBulletLine)
                );
                Chart.DrawTrendLine(
                    "SilverBulletDown", startX, low, Math.Min(endX, Bars.Count - 1), low,
                    GetColors(SilverBulletColor), 1, GetLineStyle(SilverBulletLine)
                );
            }

            if(Server.Time >= Bars.OpenTimes[startX])
            {
                Chart.DrawText("SilverBulletLabel", "Silver Bullet", startX, high + 20, GetColors(SilverBulletColor));
            }
        }
        private void DrawDailyOpen(DateTime today)
        {
            int openIndex = Bars.OpenTimes.GetIndexByTime(today);

            if (openIndex < 0 ) return;

            double dailyOpenPrice = Bars.OpenPrices[openIndex];
            Chart.DrawTrendLine(
                "DailyOpen", openIndex,
                dailyOpenPrice, Bars.Count - 1,
                dailyOpenPrice, GetColors(DailyOpenColor), 1, GetLineStyle(DailyOpenLine)
            );
        }
        private void DrawWeeklyOpen(DateTime week)
        {
            int openIndex = Bars.OpenTimes.GetIndexByTime(week);

            if (openIndex < 0 ) return;

            double dailyOpenPrice = Bars.OpenPrices[openIndex];
            Chart.DrawTrendLine(
                "WeeklyOpen", openIndex,
                dailyOpenPrice, Bars.Count - 1,
                dailyOpenPrice, GetColors(WeeklyOpenColor), 1, GetLineStyle(WeeklyOpenLine)
            );
        }
        private void DrawMonthlyOpen(DateTime month)
        {
            int openIndex = Bars.OpenTimes.GetIndexByTime(month);

            if (openIndex < 0 ) return;

            double dailyOpenPrice = Bars.OpenPrices[openIndex];
            Chart.DrawTrendLine(
                "MonthlyOpen", openIndex,
                dailyOpenPrice, Bars.Count - 1,
                dailyOpenPrice, GetColors(MonthlyOpenColor), 1, GetLineStyle(MonthlyOpenLine)
            );
        }
        private static Color GetColors(SessionColor color) => color switch
        {
            SessionColor.Red => Color.Red,
            SessionColor.Green => Color.Green,
            SessionColor.Blue => Color.Blue,
            SessionColor.Yellow => Color.Yellow,
            SessionColor.Orange => Color.Orange,
            SessionColor.Purple => Color.Purple,
            SessionColor.Pink => Color.Pink,
            SessionColor.Cyan => Color.Cyan,
            SessionColor.Brown => Color.Brown,
            SessionColor.DarkRed => Color.DarkRed,
            SessionColor.White => Color.White,
            SessionColor.Black => Color.Black,
            SessionColor.Gray => Color.Gray,
            SessionColor.Indigo => Color.Indigo,
            SessionColor.LightBlue => Color.LightBlue,
            SessionColor.LightGreen => Color.LightGreen,
            SessionColor.LightGray => Color.LightGray,
            SessionColor.LightPink => Color.LightPink,
            SessionColor.LightYellow => Color.LightYellow,
            SessionColor.DarkBlue => Color.DarkBlue,
            SessionColor.DarkGreen => Color.DarkGreen,
            SessionColor.DarkGray => Color.DarkGray,
            SessionColor.DarkOrange => Color.DarkOrange,
            _ => Color.White
        };
        private static LineStyle GetLineStyle(Lines style) => style switch
        {
            Lines.Solid => LineStyle.Solid,
            Lines.Dots => LineStyle.Dots,
            Lines.Dashes => LineStyle.Lines,
            Lines.DashDot => LineStyle.LinesDots,
            Lines.DotsRare => LineStyle.DotsRare,
            Lines.DotsVeryRare => LineStyle.DotsVeryRare,
            _ => LineStyle.Solid
        };
    }
}