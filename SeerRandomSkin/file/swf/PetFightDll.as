package
{
   import flash.display.Sprite;
   import com.robot.core.net.SocketConnection;
   import com.robot.core.CommandID;
   import com.robot.core.dispatcher.FightDispatcher;
   import com.robot.core.event.PetFightEvent;
   import com.robot.core.info.fightInfo.attack.FightOverInfo;
   import org.taomee.events.SocketEvent;
   
   public class EveryDay extends Sprite
   {
      public function EveryDay()
      {
         super();
         SocketConnection.addCmdListener(CommandID.FIGHT_OVER,
            function(event:SocketEvent) : void
            {
                var overData:FightOverInfo = event.data as FightOverInfo;
                SocketConnection.removeCmdListener(CommandID.FIGHT_OVER,arguments.callee);
                FightDispatcher.dispatchEvent(new PetFightEvent(PetFightEvent.ALARM_CLICK,overData));
            }
         );
         SocketConnection.send(CommandID.READY_TO_FIGHT);
      }
   }
}
