/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM [dbo].[UserRoleMaps] where UserId in  ('4f6beb11-f3d7-4891-b500-a1e939e45502','f2ed3230-65d0-4473-8913-623adcd5410e')

 SELECT *
  FROM [dbo].[Users] where id in ('4f6beb11-f3d7-4891-b500-a1e939e45502','f2ed3230-65d0-4473-8913-623adcd5410e')


delete
  FROM [dbo].[UserRoleMaps] where UserId in  ('4f6beb11-f3d7-4891-b500-a1e939e45502','f2ed3230-65d0-4473-8913-623adcd5410e')
delete
  FROM [dbo].[Users] where id in ('4f6beb11-f3d7-4891-b500-a1e939e45502','f2ed3230-65d0-4473-8913-623adcd5410e')