ALTER TABLE [dbo].[Documents]
    ADD CONSTRAINT [FK_Documents_Agents_AgentId] FOREIGN KEY ([AgentId]) REFERENCES [dbo].[Agents] ([Id]) ON DELETE NO ACTION;

