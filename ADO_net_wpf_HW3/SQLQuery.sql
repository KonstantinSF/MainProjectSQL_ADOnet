use BagShopDB
--select * from Sale

--select SaleDate, Product.Name as [ProductName], Emploee.fio as [Emploee Name], NumOfProduct, Discount
--from Sale as s 
--join Product on ProductId=S.ProductId 
--join Emploee on Emploee.id=S.EmploeeID
--order by SaleDate desc

--select Operation, CreateAt as [Date of Creation], Product.Name
--from History
--join Product on Product.id=History.ProductId
--order by [Date of Creation] desc

Select SaleDate, Product.Name as [ProductName], Emploee.fio as [Emploee Name], NumOfProduct, Discount from Sale as s                    
join Product on ProductId=S.ProductId       
join Emploee on Emploee.id=S.EmploeeID 
where Emploee.fio like 'D%'
order by SaleDate desc 