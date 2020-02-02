CREATE TABLE [dbo].[Categories] (
    [SeqNo]        INT              IDENTITY (1, 1) NOT NULL,
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Name]         NVARCHAR (100)   NOT NULL,
    [PayType]      TINYINT          NOT NULL,
    [UserId]       NVARCHAR (128)   NOT NULL,
    [CreatedTime]  DATETIME2 (0)    NOT NULL,
    [ModifiedTime] DATETIME2 (0)    NOT NULL,
    [DeletedTime]  DATETIME2 (0)    NULL,
    [IsDisabled]   BIT              NOT NULL,
    [SortNumber]   INT              NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Categories_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Categories', @level2type = N'COLUMN', @level2name = N'SeqNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Categories', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Categories', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收支出類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Categories', @level2type = N'COLUMN', @level2name = N'PayType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會員編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Categories', @level2type = N'COLUMN', @level2name = N'UserId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Categories', @level2type = N'COLUMN', @level2name = N'CreatedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後異動時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Categories', @level2type = N'COLUMN', @level2name = N'ModifiedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'刪除時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Categories', @level2type = N'COLUMN', @level2name = N'DeletedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否停用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Categories', @level2type = N'COLUMN', @level2name = N'IsDisabled';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Categories', @level2type = N'COLUMN', @level2name = N'SortNumber';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'交易類別檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Categories';

