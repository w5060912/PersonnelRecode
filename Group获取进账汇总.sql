

select IncomeSource 来源 ,COUNT (IncomeAmount) as 次数,sum(IncomeAmount)  共 from IncomeRecord
group by IncomeSource

declare @i int =0
select IncomeRecord.IncomeSource from IncomeRecord
select IncomeSource  from IncomeRecord
group by IncomeSource

select @i =count