===0_1_2_front_desk===

#show Player Player
#show FD FD

FD：...是是是，艾拉姐！人到了，看着像个学生...拿着个怪球？...明白，就说您被污水厂紧急采访拖住了，让他自己...啊？拍沉淀物？记下了记下了！

她看到你，马上挂断电话，深吸一口气，挤出职业笑容。

FD：哎呀同学久等啦！艾拉小姐刚被污水厂紧急采访叫走了！她说特别抱歉，让你直接去新城区找她就行！

她递给你一份城区地图和一张名片

FD：就这个地址，好找的很！

Player：...?

#show_popup("Received Map")
#Enable(Map)
#show_popup("Received Ella name Card")
#add_notebook(Ella_name_card)

你接过便签，手机突然在口袋里剧烈震动。

Player ：污水厂紧急采访？真的吗...?

你手机屏幕亮起，一封新邮件。发件人：[数据删除]，主题：给拿球的人

#show_popup("Received UnknownMail")
#add_notebook(UnknownMail)
#show Item UnknownMail

邮件正文：
科考站地下三层B区，循环水箱取样口。真实水样。别信他们的‘达标’报告。砷吸附数据被手动平滑了。 附件是原始曲线。
（附件：一份砷浓度监测折线图。大部分是平缓绿线，但一点被红圈标注，数值陡峭飙升，旁边小字批注：样本点：渔村马家井 - 7/14。）

Player (瞳孔微缩，手指划过屏幕上那根刺眼的红线)：
【内心独白】 手动平滑？...邱？Professor提过的天才采样员，叛出项目那个？他给我发邮件？砷...渔村马家井？ 

-> END
