USE [DemographicsDb]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 31.05.2020 10:41:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StreetName] [nvarchar](40) NOT NULL,
	[HouseNumber] [nvarchar](6) NOT NULL,
	[Town] [nvarchar](40) NOT NULL,
	[State] [nvarchar](20) NOT NULL,
	[ZIPCode] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patient]    Script Date: 31.05.2020 10:41:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patient](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GivenName] [nvarchar](50) NOT NULL,
	[FamilyName] [nvarchar](50) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[SexId] [int] NOT NULL,
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientAddress]    Script Date: 31.05.2020 10:41:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientAddress](
	[PatientId] [int] NOT NULL,
	[AddressId] [int] NOT NULL,
 CONSTRAINT [PK_PatientAddress] PRIMARY KEY CLUSTERED 
(
	[PatientId] ASC,
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientPhoneNumbers]    Script Date: 31.05.2020 10:41:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientPhoneNumbers](
	[PatientId] [int] NOT NULL,
	[PhoneNumberId] [int] NOT NULL,
 CONSTRAINT [PK_PatientPhoneNumbers] PRIMARY KEY CLUSTERED 
(
	[PatientId] ASC,
	[PhoneNumberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhoneNumber]    Script Date: 31.05.2020 10:41:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneNumber](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_PhoneNumber] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sex]    Script Date: 31.05.2020 10:41:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sex](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Sex] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_Sex] FOREIGN KEY([SexId])
REFERENCES [dbo].[Sex] ([Id])
GO
ALTER TABLE [dbo].[Patient] CHECK CONSTRAINT [FK_Patient_Sex]
GO
ALTER TABLE [dbo].[PatientAddress]  WITH CHECK ADD  CONSTRAINT [FK_PatientAddress_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientAddress] CHECK CONSTRAINT [FK_PatientAddress_Address]
GO
ALTER TABLE [dbo].[PatientAddress]  WITH CHECK ADD  CONSTRAINT [FK_PatientAddress_Patient] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patient] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientAddress] CHECK CONSTRAINT [FK_PatientAddress_Patient]
GO
ALTER TABLE [dbo].[PatientPhoneNumbers]  WITH CHECK ADD  CONSTRAINT [FK_PatientPhoneNumber_Patient] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patient] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientPhoneNumbers] CHECK CONSTRAINT [FK_PatientPhoneNumber_Patient]
GO
ALTER TABLE [dbo].[PatientPhoneNumbers]  WITH CHECK ADD  CONSTRAINT [FK_PatientPhoneNumber_PhoneNumber] FOREIGN KEY([PhoneNumberId])
REFERENCES [dbo].[PhoneNumber] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientPhoneNumbers] CHECK CONSTRAINT [FK_PatientPhoneNumber_PhoneNumber]
GO
