// @flow

import normalizeValue from '../normalizeValue';

describe('StyleSheet/normalizeValue', () => {
  it('normalizes property values requiring units', () => {
    expect(normalizeValue('margin', 0)).toEqual('0px');
  });
  it('ignores unitless property values', () => {
    expect(normalizeValue('flexGrow', 1)).toEqual(1);
    expect(normalizeValue('scale', 2)).toEqual(2);
  });
});
