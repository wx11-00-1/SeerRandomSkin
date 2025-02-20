package
{
   import com.robot.core.CommandID;
   import com.robot.core.info.UserInfo;
   import com.robot.core.manager.PetManager;
   import com.robot.core.net.SocketConnection;
   import flash.display.MovieClip;
   import flash.utils.ByteArray;
   import org.taomee.events.SocketEvent;
   import flash.external.ExternalInterface;
   import com.robot.core.manager.MainManager;
   import com.robot.app.task.petstory.util.KTool;
   import com.robot.core.info.pet.PetStorage2015PetInfo;
   import com.codecatalyst.promise.Promise;
   import com.robot.core.info.pet.PetInfo;
   import flash.utils.getDefinitionByName;
   import com.codecatalyst.promise.Deferred;
   import com.robot.core.manager.UserInfoManager;
   import flash.utils.IDataInput;
   import flash.utils.Dictionary;
   
   [Embed(source="/_assets/assets.swf", symbol="item")]
   public dynamic class item extends MovieClip
   {
       
      
      public function item()
      {
         super();

         SocketConnection.WxCallback = function(result:* = null):void { ExternalInterface.call("WxSc.Priv.res",result); }

         // 隐藏其他用户
         getDefinitionByName("com.robot.app.toolBar.ToolBarController").showOrHideAllUser(false);

         // 关电池
         SocketConnection.send(41162,0);

         // 治疗背包所有精灵
         ExternalInterface.addCallback("WxCureAll",function():void {
            for each (var p:PetInfo in PetManager.allInfos) {
                if (p.hp < p.maxHp || !PetManager.getPetAllSkillPPIsFull(p)) {
                    SocketConnection.send(CommandID.PET_ONE_CURE,p.catchTime);
                }
            }
         });

         // readyData 无法直接传递到 js 层
         SocketConnection.addCmdListener(CommandID.NOTE_READY_TO_FIGHT,function(event:SocketEvent):void
        {
            var readyData = event.data;
            var ps:Array = []; // 记录所有出战精灵的血量、技能等信息
            var cur:uint = 0;
            for each (var petInfo:PetInfo in readyData.userInfos.myInfo.petInfoArr) {
                var pet:Object = new Object();
                pet.id = petInfo.id;
                pet.catchTime = petInfo.catchTime;
                pet.hp = petInfo.hp;
                pet.skillArray = [];
                for (var i:int = 0; i < Math.min(4,petInfo.skillNum); ++i, ++cur) {
                    pet.skillArray.push(readyData.userInfos.allSkillID[cur]);
                }
                pet.hideSKill = petInfo.hideSKill;
                ps.push(pet);
            }
            SocketConnection.WxOs["_rdData"] = ps;
        });

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
                   SocketConnection.WxCallback(allInfo);
                }
            });
         };
         ExternalInterface.addCallback("WxGetStoragePets",function():void { SocketConnection.WxGetStoragePets([]); });
         // 背包
        ExternalInterface.addCallback("WxClearBag",function():void
        {
            var bagBoth:Array = PetManager.getBagMap(true);
            if (bagBoth.length > 0) {
                var promises:Array = [];
                for each(var pet in bagBoth) { promises.push(PetManager.bagToInStorage(pet.catchTime)); }
                Promise.all(promises).then(function():void { SocketConnection.WxCallback(); });
            }
            else {
                SocketConnection.WxCallback();
            }
        });
        ExternalInterface.addCallback("WxSetBag1",function(bag:Array):void
        {
            if (bag.length > 0) {
                var promises:Array = [];
                for each(var pet in bag) { promises.push(PetManager.storageToInBag(pet)); }
                Promise.all(promises).then(function():void { SocketConnection.WxCallback(); });
            }
            else {
                SocketConnection.WxCallback();
            }
        });
        ExternalInterface.addCallback("WxSetBag2",function(bag:Array):void
        {
            if (bag.length > 0) {
                var promises:Array = [];
                for each(var pet in bag) { promises.push(PetManager.storageToSecondBag(pet)); }
                Promise.all(promises).then(function():void { SocketConnection.WxCallback(); });
            }
            else {
                SocketConnection.WxCallback();
            }
        });

         ExternalInterface.addCallback("WxSetTitle", function(title:uint):void
         {
            if (MainManager.actorInfo.curTitle != title) {
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
         }
         );

            SocketConnection.WxCopyFireByUserID = function(userid:int):void { SocketConnection.send(CommandID.FIRE_ACT_COPY,userid); }
            // 借火（默认绿火）
            ExternalInterface.addCallback("WxCopyFireFromMap", function(fireTypes:Array = null):void
            {
                if (fireTypes == null) {
                    fireTypes = [5,6];
                }
                SocketConnection.sendWithCallback(CommandID.LIST_MAP_PLAYER,function(param1:SocketEvent):void
                {
                    var byteArray:ByteArray = param1.data as ByteArray;
                    var len:uint = byteArray.readUnsignedInt();
                    var userInfo:UserInfo = new UserInfo();
                    for (var i:int = 0; i < len; ++i)
                    {
                        UserInfo.setForPeoleInfo(userInfo,byteArray);
                        if (fireTypes.indexOf(userInfo.fireBuff) != -1)
                        {
                            SocketConnection.WxCopyFireByUserID(userInfo.userID);
                            SocketConnection.WxCallback(true);
                            return;
                        }
                    }
                    SocketConnection.WxCallback(false);
                });
            });
            // 排行榜
            ExternalInterface.addCallback("WxGetRankListLen", function(key:int, sub_key:int):void
            {
                // key：120 竞技，182 狂野，157 图鉴（新增积分）
                KTool.getRankListLen(key,sub_key,function(len:int):void
                {
                    SocketConnection.WxCallback(len);
                });
            });
            // 向排行榜上的活跃玩家借火
            ExternalInterface.addCallback("WxCopyFireFromRank", function(key:int, sub_key:int, offset:int, fireTypes:Array = null):void
            {
                if (fireTypes == null) {
                    fireTypes = [5,6];
                }
                KTool.getBingLieRangeRankList(key,sub_key,offset,offset+99).then(function(l:Array):void
                {
                    // 获取其中的在线玩家
                    var userIDs:Array = [];
                    for each (var o:Object in l) {
                        userIDs.push(o.userid);
                    }
                    var promises:Array = [];
                    UserInfoManager.seeOnLine(userIDs,function(onlineSeer:Array):void
                    {
                        if (onlineSeer.length == 0) {
                            SocketConnection.WxCallback(false);
                            return;
                        }
                        for each (var s:Object in onlineSeer) {
                            promises.push(SocketConnection.WxCopyFirePromise(s.userID,fireTypes));
                        }
                        Promise.all(promises).then(function(results:Array):void
                        {
                            SocketConnection.WxCallback(results.indexOf(true) != -1);
                        });
                    });
                });
            });
            SocketConnection.WxCopyFirePromise = function(userid:int, fireTypes:Array):Promise
            {
                var d:Deferred = new Deferred();
                SocketConnection.sendWithPromise(CommandID.GET_SIM_USERINFO,[userid]).then(function(event:SocketEvent):void
                {
                    var userInfo:UserInfo = new UserInfo();
                    UserInfo.setForSimpleInfo(userInfo,event.data as IDataInput);
                    if (fireTypes.indexOf(userInfo.fireBuff) != -1) {
                        SocketConnection.WxCopyFireByUserID(userInfo.userID);
                        d.resolve(true);
                    }
                    else {
                        d.resolve(false);
                    }
                });
                return d.promise;
            };

         // 发包函数
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
                SocketConnection.WxCallback(packet);
            }
            ,parameterArray);
         });

         SocketConnection.WxOs = new Dictionary(); // 对象池。使用场景：用js代码创建as3对象，作为参数传递给as3函数
         ExternalInterface.addCallback("WxAddObj",function(key:String,name:String,... rest):void {
            var c:Class = getDefinitionByName(name) as Class;
            // 构造函数
            switch(rest.length) {
                case 0:
                    SocketConnection.WxOs[key] = new c();
                    break;
                case 2:
                    SocketConnection.WxOs[key] = new c(rest[0] ? SocketConnection.WxOs[rest[1]] : rest[1]);
                    break;
                case 4:
                    SocketConnection.WxOs[key] = new c(rest[0] ? SocketConnection.WxOs[rest[1]] : rest[1],rest[2] ? SocketConnection.WxOs[rest[3]] : rest[3]);
                    break;
                case 6:
                    SocketConnection.WxOs[key] = new c(rest[0] ? SocketConnection.WxOs[rest[1]] : rest[1],rest[2] ? SocketConnection.WxOs[rest[3]] : rest[3],rest[4] ? SocketConnection.WxOs[rest[5]] : rest[5]);
                    break;
                case 8:
                    SocketConnection.WxOs[key] = new c(rest[0] ? SocketConnection.WxOs[rest[1]] : rest[1],rest[2] ? SocketConnection.WxOs[rest[3]] : rest[3],rest[4] ? SocketConnection.WxOs[rest[5]] : rest[5],rest[6] ? SocketConnection.WxOs[rest[7]] : rest[7]);
                    break;
                case 10:
                    SocketConnection.WxOs[key] = new c(rest[0] ? SocketConnection.WxOs[rest[1]] : rest[1],rest[2] ? SocketConnection.WxOs[rest[3]] : rest[3],rest[4] ? SocketConnection.WxOs[rest[5]] : rest[5],rest[6] ? SocketConnection.WxOs[rest[7]] : rest[7],rest[8] ? SocketConnection.WxOs[rest[9]] : rest[9]);
                    break;
                default:
                    ExternalInterface.call("console.log","参数过多");
                    break;
            }
         });
         ExternalInterface.addCallback("WxAddFunc",function(k1:String,k2:String):void {
            SocketConnection.WxOs[k1] = function(... rest):void {
                SocketConnection.WxOs[k2] = rest;
                ExternalInterface.call("WxSc.Priv."+k1);
            }
         });
         ExternalInterface.addCallback("WxDelObj",function(k:String):void {SocketConnection.WxOs[k]=undefined;});
         
         ExternalInterface.addCallback("WxRefl",function(type:uint,name:String,path:String,... rest):* {
            var keys:Array = path.split(".");  // 使用 `.` 分隔路径
            var lastKey:String = keys.pop();   // 获取最后一个属性的键
            var current:Object = getDefinitionByName(name);
            for each (var key:String in keys) {
                current = current[key]; // 访问嵌套的对象
            }
            switch(type) {
                case 1: // 设置属性的值
                    current[lastKey] = rest[0] ? SocketConnection.WxOs[rest[1]] : rest[1]; return;
                case 2: // 获取
                    return current[lastKey];
                case 3: // 调用方法
                case 4: // 将返回值暂存到 对象池（应对一些无法直接输出到js层的对象）
                    var ps:Array = []; // 真正要传的参数。一个 ps 元素对应两个 rest 元素；每个元素都要伴随一个标志位，标志是否从 对象池 里面找（还是不能传 Function 类型的参数）
                    var i:int = type - 3;
                    for (; i < rest.length; i += 2) ps.push(rest[i] ? SocketConnection.WxOs[rest[i+1]] : rest[i+1]);
                    if (type==3) return current[lastKey].apply(null,ps);
                    else SocketConnection.WxOs[rest[0]] = current[lastKey].apply(null,ps);
                    break;
            }
         });

         getDefinitionByName("flash.utils.setTimeout").apply(null,[function():void {SocketConnection.send(CommandID.NONO_FOLLOW_OR_HOOM,0);},800]); // 将 nono 丢回仓库
         ExternalInterface.call("WxSc._in");
         ExternalInterface.call("seerRandomSkinObj.onLogined");
      }
   }
}
