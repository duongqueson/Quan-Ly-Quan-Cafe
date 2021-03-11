create database quanlyquancafe
go

use quanlyquancafe
go

alter table TableFood
(
	id int identity primary key,
	name nvarchar(100) not null default N'Bàn chưa đặt tên',
	status nvarchar(100) not null default N'Trống'	-- Trong || co nguoi
)
go

create table Account
(
	DisplayName nvarchar(100) not null default N'NoName',
	UserName nvarchar(100) primary key,
	PassWord nvarchar(1000) not null default 123456,
	Type int not null default 0		-- 0: staff || 1: admin
)
go

create table FoodCategory
(
	id int identity primary key,
	name nvarchar(100) not null default N'Chua dat ten'
)
go

create table Food
(
	id int identity primary key,
	name nvarchar(100) not null default N'Chua dat ten',
	idCategory int not null,
	price float not null,

	foreign key (idCategory) references dbo.FoodCategory(id)
)
go

create table Bill
(
	id int identity primary key,
	DateCheckin datetime not null default getdate(),
	DateCheckout datetime not null,
	idTable int not null,
	status int not null,	-- 1: da thanh toan || 0: chua thanh toan

	foreign key (idTable) references dbo.TableFood(id)
)
go

create table BillInfo
(
	id int identity primary key,
	idBill int not null,
	idFood int not null,
	count int not null default 0,

	foreign key (idBill) references dbo.Bill(id),
	foreign key (idFood) references dbo.Food(id)
)
go

CREATE PROC USP_GetAccountByUserName
@userName nvarchar(100)
AS 
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName
END
GO

EXEC dbo.USP_GetAccountByUserName @userName = N'son' -- nvarchar(100)
go

select * from dbo.Account where UserName=N'son' and PassWord=N'123456'
go

create proc usp_Login
@userName nvarchar(100),@passWord nvarchar(100)
as
begin
	select * from dbo.Account where UserName=@userName and PassWord=@passWord
end
go

exec dbo.usp_Login @userName=N'son' , @passWord=N'123456'
go

create proc usp_GetTableList
as select * from dbo.tablefood
go

exec dbo.usp_GetTableList
go

create proc usp_GetListFreeTable
as select * from dbo.tablefood where status=N'Trống'
go

exec usp_GetListFreeTable


-- thêm bill
INSERT	dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idTable ,
          status
        )
VALUES  ( GETDATE() , -- DateCheckIn - date
          NULL , -- DateCheckOut - date
          3 , -- idTable - int
          0  -- status - int
        )
        
INSERT	dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idTable ,
          status
        )
VALUES  ( GETDATE() , -- DateCheckIn - date
          NULL , -- DateCheckOut - date
          4, -- idTable - int
          0  -- status - int
        )
INSERT	dbo.Bill
        ( DateCheckIn ,
          DateCheckOut ,
          idTable ,
          status
        )
VALUES  ( GETDATE() , -- DateCheckIn - date
          GETDATE() , -- DateCheckOut - date
          5 , -- idTable - int
          1  -- status - int
        )

-- thêm bill info
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 5, -- idBill - int
          1, -- idFood - int
          2  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 5, -- idBill - int
          3, -- idFood - int
          4  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 5, -- idBill - int
          5, -- idFood - int
          1  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 6, -- idBill - int
          1, -- idFood - int
          2  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 6, -- idBill - int
          6, -- idFood - int
          2  -- count - int
          )
INSERT	dbo.BillInfo
        ( idBill, idFood, count )
VALUES  ( 7, -- idBill - int
          5, -- idFood - int
          2  -- count - int
          )         
          
GO
--
select * from dbo.Bill where idTable=4 and status=0
--
select * from dbo.BillInfo where idBill=5
--
select f.name,bi.count,f.price,f.price*bi.count as tottalPrice from dbo.Bill as b,dbo.BillInfo as bi,dbo.Food as f
where bi.idBill=b.id and bi.idFood=f.id and b.idTable=4
--
 
 --Thêm bill mới
 create proc usp_InsertBill
 @idTable int
 as
 begin
	insert into dbo.Bill
				(	DateCheckin,
					DateCheckout,
					idTable,
					status
				)
		values	(	GETDATE(),
					null,
					@idTable,
					0
				)
 end
 go

exec usp_InsertBill @idTable
go
------------
alter proc usp_InsertBillInfo
@idBill int,@idFood int,@count int
as
begin
	declare @isExitsBillInfo int 
	declare @foodCount int = 1

	select @isExitsBillInfo = id,@foodCount = b.count
	from dbo.BillInfo as b
	where idBill=@idBill and idFood=@idFood

	if(@isExitsBillInfo > 0)
	begin
		declare @newCount int = @foodCount +  @count
		if(@newCount > 0)
			update dbo.BillInfo set count = @foodCount + @count where idFood=@idFood
		else
			delete dbo.BillInfo where idBill=@idBill and idFood=@idFood
	end
	else
		insert into dbo.BillInfo
					(	idBill,
						idFood,
						count
					)
			values	(	@idBill,
						@idFood,
						@count
					)
end
go

exec usp_InsertBillInfo @idBill=4,@idFood=1,@count=-1
select * from dbo.BillInfo
go
-----------------------
 update dbo.TableFood set status=N'Có người' where id=2

------------------------
ALTER PROC USP_InsertBill
@idTable INT
AS
BEGIN
	INSERT dbo.Bill 
	        ( DateCheckIn ,
	          DateCheckOut ,
	          idTable ,
	          status,
			  discount
	        )
	VALUES  ( GETDATE() , -- DateCheckIn - date
	          NULL , -- DateCheckOut - date
	          @idTable , -- idTable - int
	          0,  -- status - int
			  0
	        )
END
GO
-----------------------------------------
CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS
BEGIN

	DECLARE @isExitsBillInfo INT
	DECLARE @foodCount INT = 1
	
	SELECT @isExitsBillInfo = id, @foodCount = b.count 
	FROM dbo.BillInfo AS b 
	WHERE idBill = @idBill AND idFood = @idFood

	IF (@isExitsBillInfo > 0)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count
		IF (@newCount > 0)
			UPDATE dbo.BillInfo	SET count = @foodCount + @count WHERE idFood = @idFood
		ELSE
			DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
	END
	ELSE
	BEGIN
		INSERT	dbo.BillInfo
        ( idBill, idFood, count )
		VALUES  ( @idBill, -- idBill - int
          @idFood, -- idFood - int
          @count  -- count - int
          )
	END
END
GO

DELETE dbo.BillInfo

DELETE dbo.Bill
go
-------------------------------------------------------------
CREATE TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = idBill FROM Inserted
	
	DECLARE @idTable INT
	
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill AND status = 0
	
	UPDATE dbo.TableFood SET status = N'Có người' WHERE id = @idTable
END
GO
-------------------------------------------------------------------
CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = id FROM Inserted	
	
	DECLARE @idTable INT
	
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill
	
	DECLARE @count int = 0
	
	SELECT @count = COUNT(*) FROM dbo.Bill WHERE idTable = @idTable AND status = 0
	
	IF (@count = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable
END
GO

alter table dbo.bill
ADD discount int

update dbo.Bill set discount=0

select * from Bill

update dbo.Bill set status=1, discount=10 where id=13
-------------------------

alter PROCEDURE usp_SwitchTable
@id1 int, @id2 int
AS
BEGIN
	DECLARE @idBill1 int

	SELECT @idBill1 = id FROM dbo.Bill WHERE dbo.Bill.idTable = @id1 AND STATUS = 0

	UPDATE dbo.Bill SET dbo.Bill.idTable = @id2 WHERE id = @idBill1
	UPDATE dbo.TableFood SET dbo.TableFood.status=N'Trống' WHERE id=@id1
	UPDATE dbo.TableFood SET dbo.TableFood.status=N'Có người' WHERE id=@id2
END

exec usp_SwitchTable @id1=1 , @id2=4
------------------------------------------
select * from TableFood
select * from Bill
-----------------------------------------

--------------------------------------------------------
 alter  proc usp_GetListBillByDate
 @checkIn date,@checkOut date
 as
 begin
	select b.id as [ID],t.name as [Tên bàn],DateCheckin as [DateCheckin],DateCheckout as [DateCheckout],discount as [Giảm giá],totalPrice as [Tổng tiền]
	from dbo.Bill as b, dbo.TableFood as t
	where DateCheckin>=@checkIn and DateCheckout<=@checkOut and b.status=1 and t.id=b.idTable
end
go

exec usp_GetListBillByDate @checkIn = '2021-03-11 16:53:28.510' , @checkOut='2021-03-12 16:54:06.610'
----------------------------------------------------------------------------------------------------------------
select * from dbo.Account where @UserName='son'

---------------------------------------------------

CREATE PROC USP_UpdateAccount
@userName NVARCHAR(100), @displayName NVARCHAR(100), @password NVARCHAR(100), @newPassword NVARCHAR(100)
AS
BEGIN
	DECLARE @isRightPass INT = 0
	
	SELECT @isRightPass = COUNT(*) FROM dbo.Account WHERE USERName = @userName AND PassWord = @password
	
	IF (@isRightPass = 1)
	BEGIN
		IF (@newPassword = NULL OR @newPassword = '')
		BEGIN
			UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @userName
		END		
		ELSE
			UPDATE dbo.Account SET DisplayName = @displayName, PassWord = @newPassword WHERE UserName = @userName
	end
END
GO

alter PROC GetListBillByDateAndPage(@checkIn DATETIME, @checkOut DATETIME, @page int)
AS 
BEGIN
	DECLARE @pageRows INT = 10
	DECLARE @selectRows INT = @pageRows
	DECLARE @exceptRows INT = (@page - 1) * @pageRows
	
	;WITH BillShow AS( SELECT Bill.ID,TableFood.name AS [Tên bàn], DateCheckin AS [DateCheckin], DateCheckout AS [DateCheckout], DISCOUNT AS [Giảm giá], totalPrice AS [Tổng tiền]
	FROM dbo.Bill, dbo.TableFood
	WHERE DateCheckin >= @checkIn AND DateCheckout <= @checkOut AND Bill.STATUS = 1  AND TableFood.id=Bill.idTable)
	SELECT TOP (@selectRows) * FROM BillShow WHERE ID NOT IN (SELECT TOP (@exceptRows) ID FROM BillShow)
END
-----------------------------------------------------------
create proc usp_CountBillByDate
 @checkIn date,@checkOut date
 as
 begin
	select count (*)
	from dbo.Bill
	where DateCheckin>=@checkIn and DateCheckout<=@checkOut and status=1
end
go

exec usp_CountBillByDate @checkIn = '2021-3-3' , @checkOut='2021-3-3'
--------------------------------------------------------------
alter proc usp_SumPrice
 @checkIn date,@checkOut date
 as
 begin
	select SUM(totalPrice)
	from dbo.Bill
	where DateCheckin>=@checkIn and DateCheckout<=@checkOut and status=1
end
go

exec usp_SumPrice @checkIn = '2021-3-3' , @checkOut='2021-3-3'
--------------------------------------------------------------------------

create FUNCTION ShowBillTableLast(@ID int) -- IDTable
RETURNS @Show TABLE (
					NameF nvarchar(30),
					Amount INT,
					Price FLOAT)
AS
BEGIN
	DECLARE @IDB INT -- ID Bill
	SELECT TOP 1 @IDB= ID FROM dbo.BILL WHERE ID=@ID AND STATUS =1
	ORDER BY ID DESC
	INSERT INTO @Show
	SELECT NAME,count,PRICE
	FROM dbo.BILLINFO INNER JOIN dbo.FOOD ON FOOD.ID=IDFOOD
	WHERE IDBILL=@IDB
	RETURN
END
