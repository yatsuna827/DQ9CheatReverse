

		// Token: 0x060001E3 RID: 483 RVA: 0x00015DF8 File Offset: 0x00013FF8
		private void RenewalDungeonDetail(TreasureMapData mapData)
		{
			if (mapData != null && mapData.MapType == MapType.Normal && mapData.Rank >= 2 && mapData.Rank <= 248)
			{
				this.richTextBox_dummy.Visible = true;
				this.textBox_DungeonDetail.Text = string.Empty;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("Rank : {0:X2}, Seed : {1:X4}\n", mapData.Rank, mapData.Seed);
				stringBuilder.AppendFormat("{0}\n", mapData.MapName);
				stringBuilder.AppendFormat("マップタイプ : {0}\n", mapData.MapTypeName);
				stringBuilder.AppendFormat("階層 : {0}\n", mapData.FloorCount);
				stringBuilder.AppendFormat("敵Rank : {0}\n", mapData.MonsterRank);
				stringBuilder.AppendFormat("ボス : {0}\n", mapData.BossName);
				int treasureBoxCount = mapData.GetTreasureBoxCount(0);
				stringBuilder.AppendFormat("宝箱の数 : {0}\n", treasureBoxCount);
				if (treasureBoxCount > 0)
				{
					stringBuilder.Append(" ランク別\n  ");
					for (int i = 10; i > 0; i--)
					{
						stringBuilder.AppendFormat(" {0}:{1}", TreasureMapDataControl._treasureBoxRankSymbol[0, i - 1], mapData.GetTreasureBoxCount(i));
					}
					stringBuilder.Append("\n");
					stringBuilder.Append(" 階層別\n");
					for (int j = 0; j < mapData.FloorCount; j++)
					{
						int treasureBoxCountPerFloor = mapData.GetTreasureBoxCountPerFloor(j);
						if (treasureBoxCountPerFloor > 0)
						{
							stringBuilder.AppendFormat("   地下{0:D2}階 :", j + 1);
							foreach (TreasureBoxInfo treasureBoxInfo in mapData.TreasureBoxInfoList[j])
							{
								stringBuilder.AppendFormat(" {0}", TreasureMapDataControl._treasureBoxRankSymbol[0, treasureBoxInfo.Rank - 1]);
							}
							stringBuilder.Append("\n");
						}
					}
				}
				for (int k = 0; k < mapData.FloorCount; k++)
				{
					this._treasureBoxCount[k] = 0;
					byte[,] floorMap = mapData.GetFloorMap(k);
					if (floorMap != null)
					{
						stringBuilder.Append("\n");
						stringBuilder.AppendFormat("地下{0:D2}階\n", k + 1);
						for (int l = 0; l < mapData.GetFloorHeight(k); l++)
						{
							for (int m = 0; m < mapData.GetFloorWidth(k); m++)
							{
								if (floorMap[l, m] == 1 || floorMap[l, m] == 3)
								{
									stringBuilder.Append("■");
								}
								else if (floorMap[l, m] == 4)
								{
									if (mapData.IsUpStep(k, m, l))
									{
										stringBuilder.Append("△");
									}
									else
									{
										stringBuilder.Append("\u3000");
									}
								}
								else if (floorMap[l, m] == 5)
								{
									stringBuilder.Append("▽");
								}
								else if (floorMap[l, m] == 6)
								{
									int num = mapData.IsTreasureBoxRank(k, m, l);
									if (num > 0)
									{
										int treasureBoxIndex = mapData.GetTreasureBoxIndex(k, m, l);
										this._treasureBoxIndexes[k, treasureBoxIndex] = stringBuilder.Length;
										stringBuilder.AppendFormat("{0}", TreasureMapDataControl._treasureBoxRankSymbol[1, num - 1]);
										this._treasureBoxCount[k]++;
									}
									else
									{
										stringBuilder.Append("\u3000");
									}
								}
								else
								{
									stringBuilder.Append("\u3000");
								}
							}
							stringBuilder.Append("\n");
						}
						for (int n = 0; n < this._treasureBoxCount[k]; n++)
						{
							int rank = mapData.TreasureBoxInfoList[k][n].Rank;
							int index = mapData.TreasureBoxInfoList[k][n].Index;
							string treasureBoxItem = mapData.GetTreasureBoxItem(k, index, 2);
							stringBuilder.AppendFormat("({0:D2}, {1:D2}) {2} {3}\n", new object[]
							{
								mapData.TreasureBoxInfoList[k][n].X,
								mapData.TreasureBoxInfoList[k][n].Y,
								TreasureMapDataControl._treasureBoxRankSymbol[1, rank - 1],
								treasureBoxItem
							});
						}
					}
				}
				this.textBox_DungeonDetail.Text = stringBuilder.ToString();
				using (Font font = new Font("ＭＳ ゴシック", 9f, FontStyle.Underline, GraphicsUnit.Point, 128))
				{
					for (int num2 = 0; num2 < mapData.FloorCount; num2++)
					{
						for (int num3 = 0; num3 < this._treasureBoxCount[num2]; num3++)
						{
							this.textBox_DungeonDetail.SelectionStart = this._treasureBoxIndexes[num2, num3];
							this.textBox_DungeonDetail.SelectionLength = 1;
							this.textBox_DungeonDetail.SelectionFont = font;
							this.textBox_DungeonDetail.SelectionColor = Color.Blue;
						}
					}
				}
				this.textBox_DungeonDetail.SelectionStart = 0;
				this.textBox_DungeonDetail.SelectionLength = 0;
				this.textBox_DungeonDetail.ScrollToCaret();
				this.label_DungeonDetail.Enabled = true;
				this.textBox_DungeonDetail.Enabled = true;
				this.richTextBox_dummy.Visible = false;
				return;
			}
			this.label_DungeonDetail.Enabled = false;
			this.textBox_DungeonDetail.Enabled = false;
			this.textBox_DungeonDetail.Text = string.Empty;
		}