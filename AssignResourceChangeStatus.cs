using Countersoft.Gemini;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;
using Countersoft.Gemini.Commons.Meta;
using Countersoft.Gemini.Extensibility.Apps;
using Countersoft.Gemini.Extensibility.Events;
using Countersoft.Gemini.Infrastructure.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AssignResourceChangeStatus
{
    [AppType(AppTypeEnum.Event),
    AppGuid("49389E25-0E54-46D4-990A-0A954D956C10"),
    AppName("Assign Resource Change Status"),
    AppDescription("Automatically set status to assigned (2nd item in status list) when you assign a resource")]
    class AssignResourceChangeStatus : IIssueBeforeListener
    {
        public Issue BeforeCreate(IssueEventArgs args)
        {
            return args.Entity;
        }

        public Issue BeforeUpdate(IssueEventArgs args)
        {
            return args.Entity;
        }

        public Issue BeforeDelete(IssueEventArgs args)
        {
            return args.Entity;
        }

        public IssueComment BeforeComment(IssueCommentEventArgs args)
        {
            return args.Entity;
        }

        public Issue BeforeStatusChange(IssueEventArgs args)
        {
            return args.Entity;
        }

        public Issue BeforeResolutionChange(IssueEventArgs args)
        {
            return args.Entity;
        }

        public Issue BeforeAssign(IssueEventArgs args)
        {
            return args.Entity;
        }

        public Issue BeforeClose(IssueEventArgs args)
        {
            return args.Entity;
        }

        public Issue BeforeWatcherAdd(IssueEventArgs args)
        {
            return args.Entity;
        }

        public IssueDto BeforeIssueCopy(IssueDtoEventArgs args)
        {
            return args.Issue;
        }

        public IssueDto CopyIssueComplete(IssueDtoEventArgs args)
        {
            return args.Issue;
        }

        public string Description { get; set; }
        public string Name { get; set; }
        public string AppGuid { get; set; }

        public IssueDto BeforeCreateFull(IssueDtoEventArgs args)
        {
            return UpdateStatus( args );
        }

        public IssueDto BeforeUpdateFull(IssueDtoEventArgs args)
        {
            return UpdateStatus(args);
        }

        private IssueDto UpdateStatus( IssueDtoEventArgs args )
        {
            //If there is no resource, then do not want to do anything
            if ( args.Issue.Entity.GetResources().Count == 0 ) return args.Issue;

            var project = args.Context.Projects.Get( args.Issue.Entity.ProjectId );

            if ( project == null ) return args.Issue;

            //If the status is NOT the default for the project/type AND default statusId has been selected then exit
            ProjectManager pm = GeminiApp.GetManager<ProjectManager>( args.User ); ;
            var projectDefaults = pm.GetDefaults( args.Issue.Project, args.Issue.Entity.TypeId );
            
            //          Current status is not the default statusId           and   there was a default status set
            if( args.Issue.Entity.StatusId != projectDefaults.Values.StatusId && projectDefaults.Values.StatusId  > 0 )
            {
                return args.Issue;
            }

            var metaManager = GeminiApp.GetManager<MetaManager>( args.User );

            var issueType = metaManager.TypeGet( args.Issue.Entity.TypeId ) ; // args.Context.Meta.TypeGet( args.Issue.Entity.TypeId );
            var issueStatus = metaManager.StatusGet( args.Issue.Entity.StatusId ); // args.Context.Meta.StatusGet( args.Issue.Entity.StatusId );

            var workflow = metaManager.StatusGetWorkflow( issueType.Entity, issueStatus.Entity, args.Issue.Entity );
            
            if ( workflow == null || workflow.Count() <= 1 )
            {
                return args.Issue;
            }

            // if the status is the first in the workflow (and default from above) then move along one
            if ( args.Issue.Entity.StatusId == workflow[0].Entity.Id )
            {
                args.Issue.Entity.StatusId = workflow[1].Entity.Id;
            }

            return args.Issue;
        }
    }
}
