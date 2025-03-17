import { test, expect } from '@playwright/test'
import { initializeTestData } from './test-utils'

test.describe('ユーザー管理画面', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/')
    await page.getByRole('link', { name: 'ユーザー管理' }).click()
  })

  test('新規ユーザー追加', async ({ page }) => {
    await page.getByRole('button', { name: '新規ユーザー追加' }).click()
    await page.getByLabel('ADアカウント').fill('suzuki.jiro')
    await page.getByLabel('表示名').fill('鈴木次郎')
    await page.getByRole('button', { name: '保存' }).click()
    await expect(page.getByRole('row', { name: 'suzuki.jiro' })).toBeVisible()
    await expect(page.getByRole('row', { name: '鈴木次郎' })).toBeVisible()
  })

  test('ユーザー編集', async ({ page }) => {
    await initializeTestData(page)
    await page.goto('/users')
    await page.getByRole('row', { name: 'yamada.taro' }).getByRole('button', { name: '編集' }).click()
    await page.getByLabel('ADアカウント').fill('yamada.taro.updated')
    await page.getByLabel('表示名').fill('山田太郎（更新）')
    await page.getByRole('button', { name: '保存' }).click()
    await expect(page.getByRole('row', { name: 'yamada.taro.updated' })).toBeVisible()
    await expect(page.getByRole('row', { name: '山田太郎（更新）' })).toBeVisible()
  })

  test('ユーザー削除', async ({ page }) => {
    await initializeTestData(page)
    await page.goto('/users')
    await page.getByRole('row', { name: 'yamada.taro' }).getByRole('button', { name: '削除' }).click()
    await page.getByRole('button', { name: '削除' }).click()
    await expect(page.getByRole('row', { name: 'yamada.taro' })).not.toBeVisible()
    await expect(page.getByRole('row', { name: '山田太郎' })).not.toBeVisible()
  })

  test('ユーザー無効化', async ({ page }) => {
    await initializeTestData(page)
    await page.goto('/users')
    await page.getByRole('row', { name: 'yamada.taro' }).getByRole('button', { name: '編集' }).click()
    await page.getByLabel('有効').click()
    await page.getByRole('button', { name: '保存' }).click()
    await expect(page.getByRole('row', { name: '無効' })).toBeVisible()
  })

  test('表題クリックでトップに戻る', async ({ page }) => {
    await page.getByRole('heading', { name: 'ユーザー管理' }).click()
    await expect(page).toHaveURL('/')
  })
}) 