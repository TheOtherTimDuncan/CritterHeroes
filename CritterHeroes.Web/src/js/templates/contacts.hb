{{#each contacts}}
<tr data-active="{{isActive}}">
    <td><span class="{{#if isPerson}}person{{/if}}{{#if isBusiness}}business{{/if}}">&nbsp;</span></td>
    <td>{{contactName}}</td>
    <td>
        {{address}}
        {{#if address}}
        <br />
        {{/if}}
        {{city}}{{#if city}}, {{/if}}{{state}}&nbsp;{{zip}}
    </td>
    <td>{{email}}</td>
    <td>{{groups}}</td>
</tr>
{{else}}
<tr>
    <td colspan="4">No contacts found.</td>
</tr>
{{/each}}
