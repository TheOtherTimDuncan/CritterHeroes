<div class="flex-table-header">
    <div class="flex-table-cell flex-cell-s2">Name</div>
    <div class="flex-table-cell flex-cell-s2">Address</div>
    <div class="flex-table-cell flex-cell-s2">Email</div>
    <div class="flex-table-cell flex-cell-s1">Groups</div>
</div>
<div class="flex-table-body">
    {{#each contacts}}
    <div class="flex-table-row" data-active="{{isActive}}">
        <div class="flex-table-cell flex-cell-s2">
            <span class="flex-table-cell-header flex-table-cell-header-md">Name</span>
            <span>
                <span class="{{#if isPerson}}icon-person{{/if}}{{#if isBusiness}}icon-house{{/if}}">&nbsp;</span>
                {{contactName}}
            </span>
        </div>
        <div class="flex-table-cell flex-cell-s2">
            <span class="flex-table-cell-header flex-table-cell-header-md">Address</span>
            <span>
                {{address}}
                {{#if address}}
                <br />
                {{/if}}
                {{city}}{{#if city}}, {{/if}}{{state}}&nbsp;{{zip}}
            </span>
        </div>
        <div class="flex-table-cell flex-cell-s2 text-break-all">
            <span class="flex-table-cell-header flex-table-cell-header-md">Email</span>
            <span>{{email}}</span>
        </div>
        <div class="flex-table-cell flex-cell-s1">
            <span class="flex-table-cell-header flex-table-cell-header-md">Groups</span>
            <span>{{groups}}</span>
        </div>
    </div>
    {{else}}
    <div>
        <div class="flex-table-cell flex-cell-s1">No contacts found.</div>
    </div>
    {{/each}}
</div>
