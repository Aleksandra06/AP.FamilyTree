// @flow

import React from 'react';
import { shallow } from 'enzyme';
import createViewComponent from '../createViewComponent';
import StyleSheet from '../StyleSheet';

const View = createViewComponent(StyleSheet);

describe('createViewComponent', () => {
  it('renders View', () => {
    const element = shallow(<View />);
    expect(element.is('div')).toEqual(true);
    expect(element.hasClass('css-bozgmv')).toEqual(true);
  });
});
