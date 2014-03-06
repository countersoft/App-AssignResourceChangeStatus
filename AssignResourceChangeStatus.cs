using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;
using Countersoft.Gemini.Commons.Meta;
using Countersoft.Gemini.Extensibility.Apps;
using Countersoft.Gemini.Extensibility.Events;
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
            if (args.Entity.GetResources().Count == 0) return args.Entity;

            var project = args.Context.Projects.Get(args.Entity.ProjectId);

            if (project == null) return args.Entity;

            List<IssueStatus> statuses = args.Context.Meta.StatusGet().FindAll(s => s.TemplateId == project.TemplateId).Where(s => !s.IsFinal).OrderBy(s => s.Sequence).ToList();

            if (statuses.Count <= 1) return args.Entity;

            if (args.Previous.GetResources().Count == 0 && args.Previous.StatusId == statuses[0].Id)
            {
                args.Entity.StatusId = statuses[1].Id;

                return args.Entity;
            }

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
            return args.Issue;
        }

        public IssueDto BeforeUpdateFull(IssueDtoEventArgs args)
        {
            return args.Issue;
        }
    }
}
