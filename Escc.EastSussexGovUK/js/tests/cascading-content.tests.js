/// <reference path="..\cascading-content.js" />
var expect = chai.expect;

describe("cascading-content.js", function () {
    it("filters out content for other urls", function () {

        var items = [{ TargetUrls: ['/this-url'] }, { TargetUrls: ['/other-url'] }];

        var filter = cascadingContentFilter();
        var result = filter.filterByUrl(items, '/this-url');

        expect(result.length).to.equal(1);
    });

    it("filters items that don't cascade", function () {

        var items = [
            { TargetUrls: ['/level1'], Cascade: true, Inherit: true },
            { TargetUrls: ['/level1/level2'], Cascade: false, Inherit: true },
            { TargetUrls: ['/level1/level2/level3'], Cascade: true, Inherit: true },
        ];

        var filter = cascadingContentFilter();
        var result = filter.filterByCascade(items, '/level1/level2/level3');

        expect(result.length).to.equal(2);
    });

    it("filters items when inherit is false", function () {

        var items = [
              { TargetUrls: ['/level1'], Cascade: true, Inherit: true },
              { TargetUrls: ['/level1/level2'], Cascade: true, Inherit: false },
              { TargetUrls: ['/level1/level2/level3'], Cascade: true, Inherit: true },
        ];

        var filter = cascadingContentFilter();
        var result = filter.filterByInherit(items, '/level1/level2/level3');

        expect(result.length).to.equal(2);

    });

    it("filters blank items based on a passed function", function () {

        var items = [{ TargetUrls: ['/this-url'] }, { TargetUrls: ['/other-url'] }];

        var filter = cascadingContentFilter();
        var result = filter.filterIfBlank(items, function (item) { return item.TargetUrls[0].indexOf('this') > -1; })

        expect(result.length).to.equal(1);
    })
});