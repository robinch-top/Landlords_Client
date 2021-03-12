using System;
using ETModel;

namespace ETHotfix
{
    //客户端已知玩家UserID 尝试读取玩家资料
    //消息内容 玩家的属性 世界位置 任务栏 技能栏 装备栏 背包
    [MessageHandler(AppType.Gate)]
    public class A1002_SetUserInfo_Handler : AMRpcHandler<A1001_SetUserInfo_REQ, A1001_SetUserInfo_ACK>
    {
        protected override async ETTask Run(Session session, A1001_SetUserInfo_REQ request, A1001_SetUserInfo_ACK response, Action reply)
        {
            try
            {
                //验证Session
                if (!GateHelper.SignSession(session))
                {
                    response.Error = ErrorCode.ERR_UserNotOnline;
                    reply();
                    return;
                }

                //获取玩家对象
                User user = session.GetComponent<SessionUserComponent>().User;
                DBProxyComponent dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
                UserInfo userInfo = await dbProxyComponent.Query<UserInfo>(user.UserID);
                
                response.Userinfo.Username = userInfo.UserName;
                response.Userinfo.Money = userInfo.Money;
                response.Userinfo.Level = userInfo.Level;

                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

    }
}