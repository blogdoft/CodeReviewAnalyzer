-- Qtd pull requests no período
select count(1)
from "PULL_REQUEST" pr 
where pr."CLOSED_DATE" between :start and :finish

select pr."TITLE"
     , prw.external_id
     , w."type"
     , parent."type"
from "PULL_REQUEST" pr 
	join "PULL_REQUEST_WORKITEMS" prw on prw.pull_request_id = pr."ID"
	join "WORKITEMS" w on w.id  = prw.workitem_id
	join "WORKITEMS_RELATION" wr on wr.left_workitem_id = w.id
	join "WORKITEMS" parent on parent.id = wr.right_workitem_id
where pr."CLOSED_DATE" between :start and :finish

-- Qtd pull requests no período
select count(1)
from "WORKITEMS" w 
	join "PULL_REQUEST_WORKITEMS" prw on prw.workitem_id = w.id
    join "PULL_REQUEST" wi_pr on wi_pr."ID" = prw.workitem_id
where wi_pr."CLOSED_DATE" between :start and :finish

select pr."EXTERNAL_ID"
from "PULL_REQUEST" pr 
   join "PULL_REQUEST_WORKITEMS" prw on prw.pull_request_id = pr."ID"
--   join "WORKITEMS" w on w.id = prw.workitem_id
where pr."CLOSED_DATE" between :start and :finish
group by 1
having count(1) > 1


select *
from "WORKITEMS" w 
	join "PULL_REQUEST_WORKITEMS" prw on prw.workitem_id = w.id
    join "PULL_REQUEST" wi_pr on wi_pr."ID" = prw.workitem_id
where wi_pr."CLOSED_DATE" between :start and :finish
order by w.external_id 

-- PRs que geraram defect
select count(1)
from "WORKITEMS" w 
	join "PULL_REQUEST_WORKITEMS" prw on prw.workitem_id = w.id
    join "PULL_REQUEST" wi_pr on wi_pr."ID" = prw.workitem_id
    join "WORKITEMS_RELATION" wr on wr.right_workitem_id = w.id
where wi_pr."CLOSED_DATE" between :start and :finish
  and w."type" <> 'Bug'
  and w."type" <> 'Defect'
  and exists 
  (
  	select 1
  	from "WORKITEMS" related
  	where related.id = wr.left_workitem_id
  	  and related."type" in ('Bug', 'Defect')
  )
  
  
select w.external_id
     , w.title
     , w."type"
     , wi_pr."TITLE"
from "WORKITEMS" w 
	join "PULL_REQUEST_WORKITEMS" prw on prw.workitem_id = w.id
    join "PULL_REQUEST" wi_pr on wi_pr."ID" = prw.workitem_id
    join "WORKITEMS_RELATION" wr on wr.right_workitem_id = w.id
where wi_pr."CLOSED_DATE" between :start and :finish  
  and exists 
  (
  	select 1
  	from "WORKITEMS" related
  	where related.id = wr.left_workitem_id
  	  and related."type" in ('Bug', 'Defect')
  )
order by w."type"









