import { test, expect } from '@playwright/test'
import { initializeTestData } from './test-utils'

test.describe('PC管理画面', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/pcs')
  })

  test('新規PC追加', async ({ page }) => {
    await initializeTestData(page)
    await page.goto('/pcs')
    await page.getByRole('button', { name: '新規PC追加' }).click()
    await page.getByLabel('管理番号').fill('PC-001')
    await page.getByLabel('モデル名').fill('ThinkPad X1')
    await page.getByLabel('OS種類').selectOption('Windows 10')
    await page.getByLabel('現在のユーザー').selectOption('山田太郎')
    await page.getByRole('button', { name: '保存' }).click()
    await expect(page.getByRole('row', { name: 'PC-001' })).toBeVisible()
    await expect(page.getByRole('row', { name: 'Windows 10' })).toBeVisible()
    await expect(page.getByRole('row', { name: '山田太郎' })).toBeVisible()
  })

  test('PC編集', async ({ page }) => {
    await initializeTestData(page)
    await page.goto('/pcs')
    await page.getByRole('button', { name: '新規PC追加' }).click()
    await page.getByLabel('管理番号').fill('PC-001')
    await page.getByLabel('モデル名').fill('ThinkPad X1')
    await page.getByLabel('OS種類').selectOption('Windows 10')
    await page.getByLabel('現在のユーザー').selectOption('山田太郎')
    await page.getByRole('button', { name: '保存' }).click()
    await page.getByRole('row', { name: 'PC-001' }).getByRole('button', { name: '編集' }).click()
    await page.getByLabel('モデル名').fill('ThinkPad X1 Carbon')
    await page.getByLabel('OS種類').selectOption('Windows 11')
    await page.getByLabel('現在のユーザー').selectOption('山田花子')
    await page.getByRole('button', { name: '保存' }).click()
    await expect(page.getByRole('row', { name: 'ThinkPad X1 Carbon' })).toBeVisible()
    await expect(page.getByRole('row', { name: 'Windows 11' })).toBeVisible()
    await expect(page.getByRole('row', { name: '山田花子' })).toBeVisible()
  })

  test('PC削除', async ({ page }) => {
    await initializeTestData(page)
    await page.goto('/pcs')
    await page.getByRole('button', { name: '新規PC追加' }).click()
    await page.getByLabel('管理番号').fill('PC-001')
    await page.getByLabel('モデル名').fill('ThinkPad X1')
    await page.getByLabel('OS種類').selectOption('Windows 10')
    await page.getByLabel('現在のユーザー').selectOption('山田太郎')
    await page.getByRole('button', { name: '保存' }).click()
    await page.getByRole('row', { name: 'PC-001' }).getByRole('button', { name: '削除' }).click()
    await page.getByRole('button', { name: '削除' }).click()
    await expect(page.getByRole('row', { name: 'PC-001' })).not.toBeVisible()
  })

  test('表題クリックでトップに戻る', async ({ page }) => {
    await page.getByRole('heading', { name: 'PC管理' }).click()
    await expect(page).toHaveURL('/')
  })
}) 