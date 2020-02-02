CREATE TABLE [dbo].[Records] (
    [SeqNo]          INT              IDENTITY (1, 1) NOT NULL,
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [CategoryItemId] UNIQUEIDENTIFIER NOT NULL,
    [TradeDate]      DATE             NOT NULL,
    [Money]          INT              NOT NULL,
    [Note]           NVARCHAR (255)   NOT NULL,
    [CreatedTime]    DATETIME2 (0)    NOT NULL,
    [ModifiedTime]   DATETIME2 (0)    NOT NULL,
    [DeletedTime]    DATETIME2 (0)    NULL,
    CONSTRAINT [PK_Records] PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Records_CategoryItems] FOREIGN KEY ([CategoryItemId]) REFERENCES [dbo].[CategoryItems] ([Id])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Records', @level2type = N'COLUMN', @level2name = N'SeqNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Records', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類別細項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Records', @level2type = N'COLUMN', @level2name = N'CategoryItemId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'交易日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Records', @level2type = N'COLUMN', @level2name = N'TradeDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Records', @level2type = N'COLUMN', @level2name = N'Money';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Records', @level2type = N'COLUMN', @level2name = N'Note';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Records', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後異動時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Records', @level2type = N'COLUMN', @level2name = N'ModifiedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'刪除時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Records', @level2type = N'COLUMN', @level2name = N'DeletedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'交易紀錄表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Records';

