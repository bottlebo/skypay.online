select *,
(select vals.Value from dbo.CategoryPropsValues as vals where vals.productId = 1 AND vals.categoryPropsId = props.Id) as val

from dbo.CategoryProps as props
--left join dbo.CategoryPropsValues as vals on props.Id = vals.categoryPropsId
