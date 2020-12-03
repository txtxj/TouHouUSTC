#### 角色属性

 - 等级 (lv)
 - 经验 (exp)
 - 体力 (gpa)
 - 最大体力 (mgpa)
 - 力量 (atk)
 - 防御 (def)
 - 魔力 (mag)
 - 技术 (tec)
 - 敏捷 (agi)
 - 移动 (mov)
 - 幸运 (luk)
 - 负重 (wgt)

#### 游戏玩法

红蓝双方对抗，平均 AGI 较高的队伍优先行动。

对抗过程与普通战棋相同。

胜利条件：击杀敌方主帅。

当 A 主动攻击 B 时，若 A 在 B 的攻击范围内，则 B 可以反击。

当 A 主动攻击 B 时，若 $A.AGI>=B.AGI*2$ ，则 A 可发动二连击。

单次伤害计算方式：

 $ Dam=(A.atk-B.def)*A.weapon.atk $

 $ Dam=A.mag*A.book.mag+A.lv-B.mag $

 $ HRate=(A.weapon.rt+A.tec*2+A.luk-B.luk+A.agi-B.agi)/100 $

 $ CRate=(A.weapon.cr+A.tec+A.luk-B.luk)/100 $

魔法命中率 75% ，必不暴击。

#### AI 思路（暂定）

 $ mov+atk+def/1.5+weapon.atk+mag*book.mag $ 值最高的棋子先移动，

求以该棋子为源的单源最短路。

若该棋子攻击范围内无敌方棋子，

则从距离最近的一个敌方棋子位置开始回溯，

直至找到一点，处于自己移动范围内，

然后移向该位置。

若攻击范围内有敌方棋子，

找到距离最近且在攻击范围内的三个敌方棋子，

估值函数：

 $ f(A,B)=(1-B.gpa/B.mgpa)*5+Dam(A,B)*Hrate(A,B)^2-Dam(B,A)*Hrate(B,A)^2-B.agi $

选择估值函数值最大的敌方棋子

以其位置为源求单源最短路

找到一个位置，使得该位置满足

A 可攻击 B ，B 不可攻击 A

并移动到此位置进行攻击。

若找不到这样的位置，

则随机选择一个位置，使得 A 可攻击到 B

并移动到此位置进行攻击。
