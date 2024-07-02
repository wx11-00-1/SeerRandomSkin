package
{
   import flash.display.Sprite;
   import flash.utils.getDefinitionByName;
   import com.robot.core.CommandID;
   import com.robot.core.info.BroadcastInfo;
   
   public class EveryDay extends Sprite
   {
       
      
      private var SocketConnection:Object;
      
      public function EveryDay()
      {
         super();
         this.SocketConnection = getDefinitionByName("com.robot.core.net.SocketConnection");
         this.SocketConnection.addCmdListener(CommandID.FIGHT_OVER,
            function() : void
            {
                var cls:* = getDefinitionByName("com.robot.app.control.BroadcastController");
                var broadcastInfo:BroadcastInfo = new BroadcastInfo();
                broadcastInfo.isLocal = true;
                broadcastInfo.type = 9999;
                broadcastInfo.txt = "<font color=\'#ff99cc\'>Õ½¶·½áÊø</font>";
                cls.addBroadcast(broadcastInfo);
            }
         );
      }
   }
}
