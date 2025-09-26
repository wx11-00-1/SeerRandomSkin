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
   import flash.utils.getQualifiedClassName;
   import flash.events.MouseEvent;
   import com.robot.core.manager.LevelManager;
   import flash.display.Loader;
   import flash.net.URLRequest;
   import flash.events.Event;
   import flash.display.DisplayObject;
   import flash.geom.Rectangle;
   import flash.display.BitmapData;
   import com.adobe.images.JPGEncoder;
   import com.adobe.images.PNGEncoder;
   import flash.net.URLRequestMethod;
   import flash.system.ApplicationDomain;
   import flash.system.LoaderContext;
   import flash.geom.Matrix;
   
   [Embed(source="/_assets/assets.swf", symbol="item")]
   public dynamic class item extends MovieClip
   {
      
      public function item()
      {
         super();

         if (SocketConnection.hasOwnProperty("WxOs")) return;

         SocketConnection.WxCallback = function(result:* = null):void { ExternalInterface.call("WxSc.Priv.res",result); }

         // ���������û�
         getDefinitionByName("com.robot.app.toolBar.ToolBarController").showOrHideAllUser(false);

         // readyData �޷�ֱ�Ӵ��ݵ� js ��
         SocketConnection.addCmdListener(CommandID.NOTE_READY_TO_FIGHT,function(event:SocketEvent):void
        {
            var readyData = event.data;
            var ps:Array = []; // ��¼���г�ս�����Ѫ�������ܵ���Ϣ
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

         // ��ȡ�ֿ⾫��
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
         // ����
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
            // ���Ĭ���̻�
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
            // ���а�
            ExternalInterface.addCallback("WxGetRankListLen", function(key:int, sub_key:int):void
            {
                // key��120 ������182 ��Ұ��157 ͼ�����������֣�
                KTool.getRankListLen(key,sub_key,function(len:int):void
                {
                    SocketConnection.WxCallback(len);
                });
            });
            // �����а��ϵĻ�Ծ��ҽ��
            ExternalInterface.addCallback("WxCopyFireFromRank", function(key:int, sub_key:int, offset:int, fireTypes:Array = null):void
            {
                if (fireTypes == null) {
                    fireTypes = [5,6];
                }
                KTool.getBingLieRangeRankList(key,sub_key,offset,offset+99).then(function(l:Array):void
                {
                    // ��ȡ���е��������
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

         // ��������
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

         SocketConnection.WxOs = new Dictionary(); // ����ء�ʹ�ó�������js���봴��as3������Ϊ�������ݸ�as3����
         ExternalInterface.addCallback("WxAddObj",function(key:String,name:String,... rest):void {
            var c:Class = getDefinitionByName(name) as Class;
            // ���캯��
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
                    ExternalInterface.call("console.log","��������");
                    break;
            }
         });
         // �ص�����
         ExternalInterface.addCallback("WxAddFunc",function(k1:String,k2:String):void {
            SocketConnection.WxOs[k1] = function(... rest):void {
                SocketConnection.WxOs[k2] = rest;
                ExternalInterface.call("WxSc.Priv."+k1);
            }
         });
         ExternalInterface.addCallback("WxDelObj",function(k:String):void {SocketConnection.WxOs[k]=undefined;});
         
         ExternalInterface.addCallback("WxRefl",function(type:uint,name:String,path:String,... rest):* {
            var keys:Array = path.split(".");  // ʹ�� `.` �ָ�·��
            var lastKey:String = keys.pop();   // ��ȡ���һ�����Եļ�
            var current:Object = getDefinitionByName(name);
            for each (var key:String in keys) {
                current = current[key]; // ����Ƕ�׵Ķ���
            }
            switch(type) {
                case 1: // �������Ե�ֵ
                    current[lastKey] = rest[0] ? SocketConnection.WxOs[rest[1]] : rest[1]; return;
                case 2: // ��ȡ
                    return current[lastKey];
                case 3: // ���÷���
                case 4: // ������ֵ�ݴ浽 ����أ�Ӧ��һЩ�޷�ֱ�������js��Ķ���
                    var ps:Array = []; // ����Ҫ���Ĳ�����һ�� ps Ԫ�ض�Ӧ���� rest Ԫ�أ�ÿ��Ԫ�ض�Ҫ����һ����־λ����־�Ƿ�� ����� �����ң����ǲ��ܴ� Function ���͵Ĳ�����
                    var i:int = type - 3;
                    for (; i < rest.length; i += 2) ps.push(rest[i] ? SocketConnection.WxOs[rest[i+1]] : rest[i+1]);
                    if (type==3) return current[lastKey].apply(null,ps);
                    else SocketConnection.WxOs[rest[0]] = current[lastKey].apply(null,ps);
                    break;
            }
         });

         // �ݴ� ����� �У�ָ����������ԣ�ͨ��Ҳ�Ƕ���
         ExternalInterface.addCallback("WxTmpAttrib",function(k1:String,attrib:String,k2:String):void {
             SocketConnection.WxOs[k2] = SocketConnection.WxOs[k1][attrib];
         });

         // �Զ�ȷ��
         ExternalInterface.addCallback("WxAutoAlarmOk",function(n:Number):void {
             getDefinitionByName("flash.utils.setInterval").apply(null,[function():void {
                 // Alarm����ս���
                 var stage:* = LevelManager.stage;
                 for (var i:int = 0; i < stage.numChildren; ++i) {
                     var child:* = stage.getChildAt(i);
                     var name:String = getQualifiedClassName(child);
                     if (name == "AlarmMC") { child["applyBtn"].dispatchEvent(new MouseEvent(MouseEvent.CLICK)); break; }
                     else if (name == "CountExpPanel_UI") { child["okBtn"].dispatchEvent(new MouseEvent(MouseEvent.CLICK)); break; }
                 }
                 // PetInBagAlert��PetInStorageAlert
                 stage = LevelManager.topLevel;
                 for (var i:int = 0; i < stage.numChildren; ++i) {
                     var child:* = stage.getChildAt(i);
                     var name:String = getQualifiedClassName(child);
                     if (name == "UI_PetSwitchAlert" || name == "UI_PetInStorageAlert") { child["applyBtn"].dispatchEvent(new MouseEvent(MouseEvent.CLICK)); break; }
                 }
                 // ItemInBagAlert
                 stage = LevelManager.tipLevel;
                 for (var i:int = 0; i < stage.numChildren; ++i) {
                     var child:* = stage.getChildAt(i);
                     var name:String = getQualifiedClassName(child);
                     if (name == "Alarm_Special" || name == "Alarm_New") { child["applyBtn"].dispatchEvent(new MouseEvent(MouseEvent.CLICK)); break; }
                 }
             },n]);
         });

         // ���� swf ��һ֡ͼƬ
         ExternalInterface.addCallback("WxSwf2Jpg", function(url:String, name:String, scaleMain:Number, scaleStage:Number):void {
            SocketConnection.WxDownloadFileName = name;
            SocketConnection.WxMcload = new Loader();
            var lc:LoaderContext = new LoaderContext(false,ApplicationDomain.currentDomain);
            SocketConnection.WxMcload.contentLoaderInfo.addEventListener(Event.COMPLETE, function(e:Event) : void {
                try {
                    var swfContent:DisplayObject = SocketConnection.WxMcload.content;
                    if (swfContent is MovieClip) {
                        MovieClip(swfContent).stop(); // ͣ���ڵ�һ֡
                    }

                    var bounds:Rectangle = swfContent.getBounds(null);
                    var bmData:BitmapData = new BitmapData(bounds.width * scaleStage, bounds.height * scaleStage, true, 0xFFFFFF);
                    // ����
                    var matrix:Matrix = new Matrix();
                    matrix.scale(scaleMain, scaleMain);
                    // ����
                    bmData.draw(swfContent, matrix);
                    // ����� ByteArray
                    // var imgBytes:ByteArray = (new JPGEncoder(100)).encode(bmData);
                    var imgBytes:ByteArray = PNGEncoder.encode(bmData);

                    // ����
                    var byteArray:Array = [];
                    imgBytes.position = 0;
                    for (var i:int = 0; i < imgBytes.length; i++)
                    {
                        byteArray.push(imgBytes.readUnsignedByte());
                    }

                    ExternalInterface.call("WxSc.Priv.DownloadJpg", byteArray,SocketConnection.WxDownloadFileName);
                } catch (err:Error) {
                    ExternalInterface.call("console.log",err.message);
                }
            });
            SocketConnection.WxMcload.load(new URLRequest(url), lc);
         });
         
         getDefinitionByName("flash.utils.setTimeout").apply(null,[function():void {SocketConnection.send(CommandID.NONO_FOLLOW_OR_HOOM,0);},800]); // �� nono ���زֿ�
         ExternalInterface.call("WxSc._in");
         ExternalInterface.call("seerRandomSkinObj.onLogined");
      }
   }
}
