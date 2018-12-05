export class ListingHelper {
   static pagination(pagesize, pageindex) {
        return '$inlinecount=allPages&$top=' + pagesize + '&$skip=' + (pageindex * pagesize);
    }

   static sort(sortby, sortdirection) {
      return  '$orderby=' + sortby + ' ' + sortdirection;
    }
}