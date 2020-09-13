﻿using System;
using System.Collections.Generic;
using EasyEvents.Types;

namespace EasyEvents.Commands
{
    public static class Spawn
    {
        public static void Run(List<string> args, int i)
        {
            if (args.Count < 1) throw new InvalidArgumentLengthException("Expected 1 argument but got 0 for command \"spawn\" at line "+i+".");

            var finalClassId = -1;
            CustomRole finalClassRole = null;
            var sum = 0;
            var classIds = new List<SpawnData>();

            for (var y = 0; y < args.Count; y++)
            {
                var argEls = args[y].Split(',');
                            
                if(argEls.Length < 1) throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");
                            
                if (argEls.Length == 1)
                {
                    if (y != args.Count - 1 || finalClassId != -1) throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");

                    var classId = -1;

                    var roleInfo = RoleInfo.parseRole(argEls[0], "spawn", i, y);
                    classId = roleInfo.classId;
                    finalClassRole = roleInfo.role;
                    
                    if(classId < 0 || classId > 17) throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");

                    finalClassId = classId;
                }
                else if (argEls.Length == 2)
                {
                    var roleInfo = RoleInfo.parseRole(argEls[0], "spawn", i, y);
                    var classId = roleInfo.classId;
                    CustomRole role = roleInfo.role;
                    
                    if(!int.TryParse(argEls[1], out var chance)) throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");
                    if(classId < 0 || classId > 17 || chance < 0 || chance > 100) throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");

                    sum += chance;
                    classIds.Add(new SpawnData(chance, new RoleInfo(role, classId)));
                }
                else throw new InvalidArgumentException("Invalid argument for command \"spawn\" on line "+i+", argument "+y+". Expected \"(0-17),(0-100)\" but got \""+args[y]+"\".");
            }
                        
            if(sum > 100) throw new InvalidArgumentException("Invalid arguments for command \"spawn\" on line "+i+", argument. The sum of spawn chances should never exceed 100. Got "+sum+".");
            if(sum < 100 && finalClassId == -1) throw new InvalidArgumentException("Invalid arguments for command \"spawn\" on line "+i+", argument. The sum of spawn chances shouldn't be less than 100 unless you have set a class to use for the remaining players. Got "+sum+".");
                        
            ScriptActions.SetCustomSpawn(classIds, new RoleInfo(finalClassRole, finalClassId), i);
        }
    }
}