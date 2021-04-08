// @flow

import { shallow } from 'enzyme';
import createDOMElement from '../createDOMElement';

describe('createDOMElement', () => {
  it('renders correct DOM element', () => {
    let element = shallow(createDOMElement('span'));
    expect(element.is('span')).toEqual(true);
    element = shallow(createDOMElement('main'));
    expect(element.is('main')).toEqual(true);
  });

  test('prop "accessibilityRole"', () => {
    const accessibilityRole = 'banner';
    let element = shallow(createDOMElement('span', { accessibilityRole }));
    expect(element.prop('role')).toEqual(accessibilityRole);
    expect(element.is('header'), true);

    const button = 'button';
    element = shallow(createDOMElement('span', { accessibilityRole: 'button' }));
    expect(element.prop('type')).toEqual(button);
    expect(element.is('button')).toEqual(true);
  });

  test('prop "accessible"', () => {
    // accessible (implicit)
    let element = shallow(createDOMElement('span', {}));
    expect(element.prop('aria-hidden')).toEqual(undefined);
    // accessible (explicit)
    element = shallow(createDOMElement('span', { accessible: true }));
    expect(element.prop('aria-hidden')).toEqual(undefined);
    // not accessible
    element = shallow(createDOMElement('span', { accessible: false }));
    expect(element.prop('aria-hidden')).toEqual(true);
  });

  test('prop "testID"', () => {
    // no testID
    let element = shallow(createDOMElement('span', {}));
    expect(element.prop('data-testid')).toEqual(undefined);
    // with testID
    const testID = 'Example.testID';
    element = shallow(createDOMElement('span', { testID }));
    expect(element.prop('data-testid')).toEqual(testID);
  });

  test('component "heading"', () => {
    // heading with number
    let element = shallow(createDOMElement('span', {
      accessibilityRole: 'heading',
      accessibilityLevel: 3,
    }));
    expect(element.is('h3')).toEqual(true);
    // heading with string
    element = shallow(createDOMElement('span', {
      accessibilityRole: 'heading',
      accessibilityLevel: '4',
    }));
    expect(element.is('h4')).toEqual(true);
    // min level is 1
    element = shallow(createDOMElement('span', {
      accessibilityRole: 'heading',
      accessibilityLevel: -1,
    }));
    expect(element.is('h1')).toEqual(true);
    // min level is 6
    element = shallow(createDOMElement('span', {
      accessibilityRole: 'heading',
      accessibilityLevel: '7',
    }));
    expect(element.is('h6')).toEqual(true);
  });
});
