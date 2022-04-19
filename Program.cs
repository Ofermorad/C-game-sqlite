using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;


namespace ofer_tmp
{
    public interface item 
    {
        string name { get; set; }
        int price { get; set; }
        bool isactive { get; set; }
        void buy ()
        {
        }
        void sell()
        {
        }
    }
    public class sword : item
    {
        public string name { get; set; }
        public int price { set; get; }
        public bool isactive { get; set; }


        public sword (string name, int price, bool isactive)
        {
            this.price = price;
            this.name = name;
            this.isactive = isactive;
        }
        public void buy ()
        {
            this.isactive = true;

        }
        public void sell ()
        {
            this.isactive = false;

        }
    }
    public class axe: item
    {
        public string name { get; set; }
        public int price { set; get; }
        public bool isactive { get; set; }

        public axe (string name, int price, bool isactive)
        {
            this.price = price;
            this.name = name;
            this.isactive= isactive;
        }
        public void buy()
        {
            this.isactive = true;

        }
        public void sell()
        {
            this.isactive = false;
        }
    }
    public class player
    {
        public string p_name;
        public item[] armory;
        public int counter;
        public int money;
        public player (string p_name)
        {
            this.p_name = p_name;
            armory= new item [2];
            this.counter = 0;
            this.money = 1000;
        }
        public void addarm (item i)
        {
            if (this.counter < 2 && this.money>=i.price)
            {
                armory[counter] = i;
                this.counter++;
                this.money -= i.price;
                i.buy();
                Console.WriteLine(this.p_name + " new wepone add : "+i.name);
                //משנה את הסכום כסף של המשתמש בבסיס הנתונים
                SQLiteConnection con = new SQLiteConnection(@"data source=C:\Users\oferm\Downloads\C#\class_interface\db\armes.db");
                con.Open();
                string quary1 = "UPDATE player_ofer SET money = @money, counter=@counter WHERE name = @name";
                SQLiteCommand cmd = new SQLiteCommand(quary1, con);
                cmd.Parameters.AddWithValue("name", this.p_name);
                cmd.Parameters.AddWithValue("money", this.money);
                cmd.Parameters.AddWithValue("counter", this.counter);
                cmd.ExecuteNonQuery();
                //יוצר טבלה עם המידע לטובת הדפסה
                string quary2 = "SELECT* from player_ofer";
                SQLiteCommand cmd2 = new SQLiteCommand(quary2, con);
                cmd2.ExecuteNonQuery();
                SQLiteDataReader reader = cmd2.ExecuteReader();
                Console.WriteLine(this.p_name +" assets:");
                while (reader.Read())
                {
                    Console.WriteLine("SQL: " + reader[0] + "," + reader[1] + "," + reader[2]);
                }
                Console.WriteLine();
                con.Close();
            }
        }
        public void sellarm(item i)
        {
            this.counter--;
            this.money += i.price;
            i.sell();
            Console.WriteLine(this.p_name + " wepone name sold: " + i.name);
            //משנה את הסכום כסף של המשתמש בבסיס הנתונים
            SQLiteConnection con = new SQLiteConnection(@"data source=C:\Users\oferm\Downloads\C#\class_interface\db\armes.db");
            con.Open();
            string quary1 = "UPDATE player_ofer SET money = @money, counter=@counter WHERE name = @name";
            SQLiteCommand cmd = new SQLiteCommand(quary1, con);
            cmd.Parameters.AddWithValue("name", this.p_name);
            cmd.Parameters.AddWithValue("money", this.money);
            cmd.Parameters.AddWithValue("counter", this.counter);
            cmd.ExecuteNonQuery();
            //יוצר טבלה עם המידע לטובת הדפסה
            string quary2 = "SELECT* from player_ofer";
            SQLiteCommand cmd2 = new SQLiteCommand(quary2, con);
            cmd2.ExecuteNonQuery();
            SQLiteDataReader reader = cmd2.ExecuteReader();
            Console.WriteLine(this.p_name + " assets:");
            while (reader.Read())
            {
                Console.WriteLine("SQL: " + reader[0] + "," + reader[1] + "," + reader[2]);
            }
            Console.WriteLine();
            con.Close();
        }
    }

    class program
    {
        public static void show_armes ()
        {

            SQLiteConnection con = new SQLiteConnection(@"data source=C:\Users\oferm\Downloads\C#\class_interface\db\armes.db");
            con.Open();
            string quary1 = "SELECT* from armes";
            SQLiteCommand cmd = new SQLiteCommand(quary1, con);
            cmd.ExecuteNonQuery();
            SQLiteDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("LIST OF ARMES IN THE GAME:");
            while (reader.Read())
                {
                    Console.WriteLine("SQL: "+reader[0]+","+reader[1]+","+reader[2]);
                }
            Console.WriteLine();
            con.Close();    
        }

        static void Main(string[] args)
        {
            sword s1 = new sword("sword of god", 10, true);
            sword a1 = new sword("ice axe", 5,true);
            player p1 = new player("ofer");
            show_armes();
            p1.addarm(s1);
            p1.addarm(a1);
            p1.sellarm(a1);
            Console.WriteLine();
        }
    }
}
