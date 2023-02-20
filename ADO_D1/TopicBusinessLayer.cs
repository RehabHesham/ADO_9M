using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_D1
{
    internal class TopicBusinessLayer
    {
        //get all
        public static DataTable GetAll()
        {
            return DatabaseLayer.Select("select * from topic");
        }

        //get by id
        public static DataTable GetById(int id)
        {
            return DatabaseLayer.Select($"select * from topic where top_id = {id}");
        }

        //add
        public static int Add(int id, string name)
        {
            return DatabaseLayer.DMLCommands($"insert into Topic values({id},'{name}')");
        }

        //edit
        public static int Edit(int id, string name)
        { 
            return DatabaseLayer.DMLCommands($"update Topic set Top_Name = '{name}' where Top_Id = {id}");
        }

        //delete
        public static int Delete(int id)
        {
            return DatabaseLayer.DMLCommands($"delete from Topic where Top_Id = {id}");
        }
    }
}
