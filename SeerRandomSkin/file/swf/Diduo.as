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
   import org.taomee.events.SocketEvent;
   import flash.external.ExternalInterface;
   import com.robot.core.info.fightInfo.attack.UseSkillInfo;
   import com.robot.core.info.fightInfo.attack.AttackValue;
   import com.robot.petFightModule_201308.view.TimerManager;
   
   [Embed(source="/_assets/assets.swf", symbol="item")]
   public dynamic class item extends MovieClip
   {
       
      
      public function item()
      {
         super();

         // 蒂朵小助手
         NpcDialog.show(5766,["你好小赛尔，这里是 SeerRandomSkin 小助手，需要什么帮助吗？"],["借绿火","自动治疗","自动出招","光年之外,我会记得我们的约定！"],[function():void
         {
            SocketConnection.sendWithCallback(CommandID.LIST_MAP_PLAYER,function(param1:SocketEvent):void
            {
               var len:uint;
               var i:int;
               var userInfo:UserInfo = null;
               var byteArray:ByteArray = param1.data as ByteArray;
               byteArray.position = 0;
               len = byteArray.readUnsignedInt();
               i = 0;
               while(i < len)
               {
                  userInfo = new UserInfo();
                  UserInfo.setForPeoleInfo(userInfo,byteArray);
                  if(userInfo.fireBuff == 5)
                  {
                     SocketConnection.send(CommandID.FIRE_ACT_COPY,userInfo.userID);
                     // setTimeout(function():void
                     // {
                     //    Alarm.show("火焰可能快失效了哦");
                     // },600000 * 2);
                     break;
                  }
                  if(userInfo.fireBuff == 6)
                  {
                     SocketConnection.send(CommandID.FIRE_ACT_COPY,userInfo.userID);
                     // setTimeout(function():void
                     // {
                     //    Alarm.show("火焰可能快失效了哦");
                     // },600000 * 2 * 3);
                     break;
                  }
                  i++;
               }
            });
         },function():void
         {
            SocketConnection.WxIsAutoCure = !SocketConnection.WxIsAutoCure;
            Alarm.show(SocketConnection.WxIsAutoCure ? "开启自动治疗" : "关闭自动治疗");
         },function():void
         {
            SocketConnection.WxIsAutoUseSkill = !SocketConnection.WxIsAutoUseSkill;
            Alarm.show(SocketConnection.WxIsAutoUseSkill ? "开始自动出招" : "停止自动出招");
         }
         ],false,null,true);
      }
   }
}
