<div class="flex-table-row flex-table-header">
  <div class="flex-table-cell flex-cell-s2">Name</div>
  <div class="flex-table-cell flex-cell-s2">Address</div>
  <div class="flex-table-cell flex-cell-s2">Email</div>
  <div class="flex-table-cell flex-cell-s1">Groups</div>
</div>
<div class="flex-table-body">
  {{#each contacts}}
  <div class="flex-table-row" data-active="{{isActive}}">
    <div class="flex-table-cell flex-cell-s2">
      <span class="{{#if isPerson}}icon-person{{/if}}{{#if isBusiness}}icon-house{{/if}}">&nbsp;</span>
      {{contactName}}
    </div>
    <div class="flex-table-cell flex-cell-s2">
      {{address}}
      {{#if address}}
      <br />
      {{/if}}
      {{city}}{{#if city}}, {{/if}}{{state}}&nbsp;{{zip}}
    </div>
    <div class="flex-table-cell flex-cell-s2 text-break-all">{{email}}</div>
    <div class="flex-table-cell flex-cell-s1">{{groups}}</div>
  </div>
  {{else}}
  <div>
    <div class="flex-table-cell flex-cell-s1">No contacts found.</div>
  </div>
  {{/each}}
</div>
