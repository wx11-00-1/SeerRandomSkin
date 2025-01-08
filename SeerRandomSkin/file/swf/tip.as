package
{
   import com.robot.core.CommandID;
   import com.robot.core.info.UserInfo;
   import com.robot.core.manager.PetManager;
   import com.robot.core.net.SocketConnection;
   import com.robot.core.npc.NpcDialog;
   import com.robot.core.ui.alert.Alarm;
   import flash.display.MovieClip;
   import flash.utils.ByteArray;
   import flash.utils.setTimeout;
   import flash.utils.JSON;
   import org.taomee.events.SocketEvent;
   import flash.external.ExternalInterface;
   import com.robot.core.manager.MainManager;
   import com.robot.core.info.fightInfo.attack.UseSkillInfo;
   import com.robot.core.info.fightInfo.attack.AttackValue;
   import com.robot.petFightModule_201308.view.TimerManager;
   import com.robot.core.info.fightInfo.ChangePetInfo;
   import com.robot.core.manager.SystemTimerManager;
   import com.robot.core.info.fightInfo.FightStartInfo;
   import com.robot.core.config.xml.ItemXMLInfo;
   import com.robot.core.config.xml.PetXMLInfo;
   import com.robot.core.config.xml.SkillXMLInfo;
   import com.robot.app.task.petstory.util.KTool;
   import com.robot.core.info.fightInfo.NoteReadyToFightInfo;
   import com.robot.core.info.fightInfo.FighterUserInfos;
   import com.robot.core.info.fightInfo.FightStartInfo;
   import com.robot.core.info.fightInfo.attack.FightOverInfo;
   import com.robot.app.toolBar.ToolBarController;
   import com.robot.core.manager.MapManager;
   import com.robot.core.manager.ModuleManager;
   import com.robot.app2.control.PeakJihad2023Controller;
   import com.robot.core.config.ClientConfig;
   import com.robot.core.config.xml.MapXMLInfo;
   import com.robot.core.info.pet.PetStorage2015PetInfo;
   import com.codecatalyst.promise.Promise;
   import com.robot.core.behavior.ChangeClothBehavior;
   import com.robot.core.manager.UserManager;
   import com.robot.core.info.clothInfo.PeopleItemInfo;
   
   [Embed(source="/_assets/assets.swf", symbol="item")]
   public dynamic class item extends MovieClip
   {
       
      
      public function item()
      {
         super();

         if (SocketConnection.WxScreenShot != null)
         {
            return;
         }

         // 获取背包精灵信息
        ExternalInterface.addCallback("WxGetBagPetInfos",
            function():Array
            {
                return PetManager.allInfos;
            }
        );

         // 地图 和 活动
         ExternalInterface.addCallback("WxChangeMap",MapManager.changeMap);
         ExternalInterface.addCallback("WxChangeMapRandom",function():uint
         {
            var mapList:Array = MapXMLInfo.getIDList();
            var id:uint = mapList[Math.round(Math.random() * mapList.length)];
            MapManager.changeMap(id);
            return id;
         });
         ExternalInterface.addCallback("WxShowAppModule",ModuleManager.showAppModule);

         // 隐藏其他用户
         ToolBarController.showOrHideAllUser(false);

         // 关电池
         SocketConnection.send(41162,0);
         // 巅峰记牌器
         SocketConnection.WxScreenShot = function() : void
         {
            ExternalInterface.call("seerRandomSkinObj.screenShot");
         };
         SocketConnection.addCmdListener(45144,SocketConnection.WxScreenShot);

         // 自动治疗
         SocketConnection.WxIsAutoCure = true;
         SocketConnection.WxAutoCureStart = function():void
         {
            SocketConnection.WxIsAutoCure = true;
         };
         SocketConnection.WxAutoCureStop = function():void
         {
            SocketConnection.WxIsAutoCure = false;
         };
         ExternalInterface.addCallback("WxAutoCureStart",SocketConnection.WxAutoCureStart);
         ExternalInterface.addCallback("WxAutoCureStop",SocketConnection.WxAutoCureStop);
         SocketConnection.WxCurePetAll = function():void
         {
            var bagBoth:Array = PetManager.getBagMap(true);
            for (var i:int = 0; i < bagBoth.length; ++i)
            {
                SocketConnection.send(CommandID.PET_ONE_CURE,bagBoth[i].catchTime);
            }
         };
         ExternalInterface.addCallback("WxCurePetAll",SocketConnection.WxCurePetAll);
         SocketConnection.WxOnFightEnd = function(event:SocketEvent) : void
         {
            if (SocketConnection.WxIsAutoCure)
            {
                SocketConnection.WxCurePetAll();
            }
            var fightOverInfo:FightOverInfo = event.data as FightOverInfo;
            ExternalInterface.call("WxFightHandler.OnFightOver",fightOverInfo);
         };
         SocketConnection.addCmdListener(CommandID.FIGHT_OVER,SocketConnection.WxOnFightEnd);

         // 使用技能
         SocketConnection.WxUseSkill = function(skillID:uint):void
         {
            SocketConnection.send(CommandID.USE_SKILL,skillID);
         };
         ExternalInterface.addCallback("WxUseSkill",SocketConnection.WxUseSkill);
         // 切换精灵
         SocketConnection.WxChangePet = function(petCatchTime:uint):void
         {
            SocketConnection.send(CommandID.CHANGE_PET,petCatchTime);
         };
         ExternalInterface.addCallback("WxChangePet",SocketConnection.WxChangePet);
         // 标记主动切换
         ExternalInterface.addCallback("WxSetIsPositiveChangePet", function():void
         {
            SocketConnection.WxIsPositiveChangePet = true;
         });
         // 使用药剂
         ExternalInterface.addCallback("WxUsePetItem", function(itemID:uint):void
         {
            SocketConnection.send(CommandID.USE_PET_ITEM,SocketConnection.WxFightingPetCatchTime,itemID,0);
         });
         ExternalInterface.addCallback("WxItemBuy", function(itemID:uint):void
         {
            SocketConnection.send(CommandID.ITEM_BUY,itemID,1);
         });

         // 获取战斗时在场精灵 ID
         ExternalInterface.addCallback("WxGetFightingPetID", function():uint
         {
            return SocketConnection.WxFightingPetID;
         });
         ExternalInterface.addCallback("WxGetFightingPetCatchTime", function():uint
         {
            return SocketConnection.WxFightingPetCatchTime;
         });
         // 根据 catchTime 获取背包中的精灵 ID
         SocketConnection.WxGetBagPetIDByCatchTime = function(catchTime:uint):uint
         {
            return PetManager.getPetInfo(catchTime).id;
         };
         ExternalInterface.addCallback("WxGetBagPetIDByCatchTime",SocketConnection.WxGetBagPetIDByCatchTime);

         // 自动出招
         // 进入战斗
         SocketConnection.WxOnReadyToFight = function():void
         {
            ExternalInterface.call("WxFightHandler.Utils.RoundReset");
            SocketConnection.WxIsPositiveChangePet = false;
         };
         SocketConnection.addCmdListener(CommandID.NOTE_READY_TO_FIGHT,SocketConnection.WxOnReadyToFight);
         // 首发精灵信息
         SocketConnection.WxOnStartFight = function(event:SocketEvent):void
         {
            var _loc2_:FightStartInfo = event.data as FightStartInfo;
            SocketConnection.WxFightingPetCatchTime = _loc2_.myInfo.catchTime;
            SocketConnection.WxFightingPetID = _loc2_.myInfo.petID;
            ExternalInterface.call("WxFightHandler.OnFirstRound",_loc2_);
         };
         SocketConnection.addCmdListener(CommandID.NOTE_START_FIGHT,SocketConnection.WxOnStartFight);
         // 使用技能
         SocketConnection.WxOnUseSkill = function(param1:SocketEvent) : void
         {
            var mySkillInfo:AttackValue;
            var enemySkillInfo:AttackValue;
            var _loc2_:UseSkillInfo = param1.data as UseSkillInfo;
            if (_loc2_.firstAttackInfo.userID == MainManager.actorInfo.userID)
            {
               mySkillInfo = _loc2_.firstAttackInfo;
               enemySkillInfo = _loc2_.secondAttackInfo;
            }
            else
            {
               mySkillInfo = _loc2_.secondAttackInfo;
               enemySkillInfo = _loc2_.firstAttackInfo;
            }
            var hpPercent:Number = enemySkillInfo.maxHp == 0 ? 0 : enemySkillInfo.remainHP * 100 / enemySkillInfo.maxHp;
            ExternalInterface.call("WxFightHandler.Utils.ShowRound",hpPercent);
            if (enemySkillInfo.remainHP == 0)
            {
                var isEnemyAllDead:Boolean = true;
                for (var i:int = 0; i < enemySkillInfo.changehps.length; ++i)
                {
                    if (enemySkillInfo.changehps[i].hp > 0)
                    {
                        isEnemyAllDead = false; break;
                    }
                }
                if (isEnemyAllDead)
                {
                    return;
                }
            }

            ExternalInterface.call("WxFightHandler.OnUseSkill",mySkillInfo,enemySkillInfo);
         };
         SocketConnection.addCmdListener(CommandID.NOTE_USE_SKILL,SocketConnection.WxOnUseSkill);
         // 切换精灵
         SocketConnection.WxOnChangePet = function(param1:SocketEvent):void
        {
            var _loc2_:ChangePetInfo = param1.data as ChangePetInfo;
            // 己方切换
            if (_loc2_.userID == MainManager.actorInfo.userID) {
                SocketConnection.WxFightingPetID = _loc2_.petID;
                SocketConnection.WxFightingPetCatchTime = _loc2_.catchTime;
                if (SocketConnection.WxIsPositiveChangePet)
                {
                    SocketConnection.WxIsPositiveChangePet = false;
                }
                else
                {
                    // 死亡切换后才能出招
                    ExternalInterface.call("WxFightHandler.OnChangePet",_loc2_);
                }
            }
        };
         SocketConnection.addCmdListener(CommandID.CHANGE_PET,SocketConnection.WxOnChangePet);

         // 压血后恢复精灵体力
         SocketConnection.WxCurePet20HP = function():void
         {
            SocketConnection.send(CommandID.ITEM_BUY,300011,6);
            SocketConnection.send(CommandID.ITEM_BUY,300017,6);
            var bag:Array = PetManager.getBagMap();
            for (var i:int = 0; i < bag.length; i++)
            {
                SocketConnection.send(CommandID.USE_PET_ITEM_OUT_OF_FIGHT,bag[i].catchTime,300011);
                SocketConnection.send(CommandID.USE_PET_ITEM_OUT_OF_FIGHT,bag[i].catchTime,300017);
            }
         };
         ExternalInterface.addCallback("WxCurePet20HP",SocketConnection.WxCurePet20HP);
         // 压血
         SocketConnection.WxLowHPFightOver = function():void
         {
            SocketConnection.removeCmdListener(CommandID.FIGHT_OVER,SocketConnection.WxLowHPFightOver);
            SocketConnection.WxAutoCureStart();
            SocketConnection.WxCurePet20HP();
         };
         SocketConnection.WxLowHP = function():void
         {
            SocketConnection.WxAutoCureStop();
            SocketConnection.send(41129, (SystemTimerManager.sysBJDate.hours < 12 || SystemTimerManager.sysBJDate.hours >= 15) ? 8692 : 8694);
            SocketConnection.addCmdListener(CommandID.FIGHT_OVER,SocketConnection.WxLowHPFightOver);
         };
         ExternalInterface.addCallback("WxLowHP",SocketConnection.WxLowHP);

         // 巅峰
         ExternalInterface.addCallback("WxPeakJihadSportsMode", function():void
         {
            if(!PeakJihad2023Controller.isInAcTime(true))
            {
                Alarm.show("还没到巅峰时间段");
               return;
            }
            if(PeakJihad2023Controller.getBagALLPetLvIsFullAndNotSame())
            {
                SocketConnection.addCmdListener(45137,function(e:SocketEvent):void
                {
                   SocketConnection.removeCmdListener(45137,arguments.callee);
                   ModuleManager.showModule(ClientConfig.getAppModule("PeakJihad2023MatchWaitPanel"),"正在打开....",3);
                });
                SocketConnection.send(45137,1,3);
            }
            else
            {
                Alarm.show("背包阵容不满足要求");
            }
         }
         );
         ExternalInterface.addCallback("WxPeakJihadWildMode", function():void
         {
            if(!PeakJihad2023Controller.isInWildAcTime(true))
            {
                Alarm.show("还没到巅峰时间段");
               return;
            }
            if(PeakJihad2023Controller.getAllBagALLPetLvIsFull())
            {
                SocketConnection.addCmdListener(45137,function(e:SocketEvent):void
                {
                   SocketConnection.removeCmdListener(45137,arguments.callee);
                   ModuleManager.showModule(ClientConfig.getAppModule("PeakJihad2023MatchWaitPanel"),"正在打开....",2);
                });
                SocketConnection.send(45137,1,2);
            }
            else
            {
                Alarm.show("背包阵容不满足要求");
            }
         }
         );
         ExternalInterface.addCallback("WxPeakJihad6v6", function():void
         {
            if(PeakJihad2023Controller.getAllBagALLPetLvIsFull())
            {
                SocketConnection.addCmdListener(45137,function(e:SocketEvent):void
                {
                   SocketConnection.removeCmdListener(45137,arguments.callee);
                   ModuleManager.showModule(ClientConfig.getAppModule("PeakJihad2023MatchWaitPanel"),"正在打开....",4);
                });
                SocketConnection.send(45137,1,11);
            }
            else
            {
                Alarm.show("背包阵容不满足要求");
            }
         }
         );
         ExternalInterface.addCallback("WxPeakJihad3v3", function():void
         {
            if(PeakJihad2023Controller.getFristBagALLPetLvIsFull2())
            {
                SocketConnection.addCmdListener(45137,function(e:SocketEvent):void
                {
                   SocketConnection.removeCmdListener(45137,arguments.callee);
                   ModuleManager.showModule(ClientConfig.getAppModule("PeakJihad2023MatchWaitPanel"),"正在打开....",1);
                });
                SocketConnection.send(45137,1,1);
            }
            else
            {
                Alarm.show("背包阵容不满足要求");
            }
         }
         );

         // 获取仓库精灵
         SocketConnection.WxGetStoragePets = function(allInfo:Array,startID:int = 1):void
         {
            SocketConnection.sendWithPromise(45543,[startID - 1,startID + 998]).then(function(e:SocketEvent):void
            {
                var b:ByteArray = e.data as ByteArray;
                var len:int = int(b.readUnsignedInt());
                for(var i:int = 0; i < len; i++)
                {
                   allInfo.push(new PetStorage2015PetInfo(b));
                }
                if(len == 1000)
                {
                   SocketConnection.WxGetStoragePets(allInfo,startID + 1000);
                }
                else
                {
                   ExternalInterface.call("WxFightHandler.Utils._as3Callback",allInfo);
                }
            });
         };
         ExternalInterface.addCallback("WxGetStoragePets", function():void
         {
            SocketConnection.WxGetStoragePets([]);
         }
         );
         // 背包
        ExternalInterface.addCallback("WxGetBag1",
            function():Array
            {
                return PetManager.getBagMap();
            }
        );
        ExternalInterface.addCallback("WxGetBag2",
            function():Array
            {
                return PetManager.getSecondBagMap();
            }
        );
         ExternalInterface.addCallback("WxSetPetBag", function(bag1:Array, bag2:Array):void
         {
            // 清空背包
            var promises:Array = new Array();
            var bagBoth:Array = PetManager.getBagMap(true);
            for (var i:int = 0; i < bagBoth.length; ++i)
            {
                promises.push(PetManager.bagToInStorage(bagBoth[i].catchTime));
            }
            Promise.all(promises).then(function():void {
                promises = [];
                // 放入精灵
                for (var i:int = 0; i < bag1.length; ++i)
                {
                    promises.push(PetManager.storageToInBag(bag1[i]));
                }
                for (var i:int = 0; i < bag2.length; ++i)
                {
                    promises.push(PetManager.storageToSecondBag(bag2[i]));
                }
                Promise.all(promises).then(function():void
                {
                    PetManager.upDateByOnce();
                    ExternalInterface.call("WxFightHandler.Utils._as3Callback");
                });
            });
         }
         );
         ExternalInterface.addCallback("WxGetClothes", function():Array
         {
            var clothes:Array = MainManager.actorInfo.clothes;
            var result:Array = [];
            for (var i:int = 0; i < clothes.length; i++)
            {
                result.push(clothes[i].id);
                result.push(clothes[i].level);
            }
            return result;
         }
         );
         ExternalInterface.addCallback("WxChangeCloth", function(clothes:Array):void
         {
            var clothArray:Array = [];
            for (var i:int = 0; i < clothes.length; i+=2)
            {
                clothArray.push(new PeopleItemInfo(clothes[i],clothes[i+1]));
            }
            MainManager.actorModel.execBehavior(new ChangeClothBehavior(clothArray));
         }
         );
         ExternalInterface.addCallback("WxGetTitle", function():uint
         {
            return MainManager.actorInfo.curTitle;
         }
         );
         ExternalInterface.addCallback("WxSetTitle", function(title:uint):void
         {
            SocketConnection.sendWithCallback(CommandID.SETTITLE,function(param1:SocketEvent):void
            {
                var _loc2_:ByteArray = null;
                if(param1.data)
                {
                    _loc2_ = param1.data as ByteArray;
                    MainManager.actorInfo.curTitle = _loc2_.readUnsignedInt();
                }
                else
                {
                    MainManager.actorInfo.curTitle = 0;
                }
                MainManager.actorModel.refreshTitle(MainManager.actorInfo.curTitle);
            },title);
         }
         );

         // 发包函数
         ExternalInterface.addCallback("WxSend",SocketConnection.send);
         ExternalInterface.addCallback("WxSendWithCallback2", function(commandID:int, parameterArray:Array = null):void
         {
            SocketConnection.sendWithCallback2(commandID,
            function(event:SocketEvent):void
            {
                var data:ByteArray = event.data as ByteArray;
                var packet:Array = new Array();
                for (var i:int = 0; i < data.length; i++)
                {
                    packet.push(data.readUnsignedByte());
                }
                ExternalInterface.call("WxFightHandler.Utils._as3Callback",packet);
            }
            ,parameterArray);
         }
         );

         // xml
         ExternalInterface.addCallback("WxGetItemNameByID", function(itemID:uint):String
         {
            return ItemXMLInfo.getName(itemID);
         }
         );
         ExternalInterface.addCallback("WxGetAllCloth", function():Array
         {
            return ItemXMLInfo.getAllCloth();
         }
         );
         ExternalInterface.addCallback("WxGetPetNameByID", function(petID:uint):String
         {
            return PetXMLInfo.getName(petID);
         }
         );
         ExternalInterface.addCallback("WxGetSkillNameByID", function(skillID:uint):String
         {
            return SkillXMLInfo.getName(skillID);
         }
         );

         // 将 nono 丢回仓库
         setTimeout(function():void
         {
             SocketConnection.send(CommandID.NONO_FOLLOW_OR_HOOM,0);
         }, 1000);
      }
   }
}
